using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class BasicEnemyMovementTargetSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const float FIELD_OF_VIEW = 120f;
		
		private readonly IOverlapExecutor _overlapExecutor;

		private ISharedData _sharedData;
		private List<int> _neighborsEntityIds;
		private int _neighborsLayer;

		private KDQuery _query = new KDQuery();

		public BasicEnemyMovementTargetSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Init(EcsSystems systems)
		{
			_neighborsEntityIds = new List<int>(16);
			_sharedData = systems.GetShared<ISharedData>();
			_neighborsLayer = LayerMask.GetMask("Enemy");
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var enemyFilter = world.Filter<EnemyComponent>()
								   .Inc<PositionComponent>()
								   .Inc<SpeedComponent>()
								   .Inc<MovementComponent>()
								   .Exc<DeadComponent>()
								   .End();
			var playerFilter = world.Filter<PlayerComponent>().Inc<PositionComponent>().End();

			var enemyComponents = world.GetPool<EnemyComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();

			var playerPosition = Vector2.zero;
			foreach (var entityId in playerFilter)
			{
				ref var positionComponent = ref positionComponents.Get(entityId);
				playerPosition = positionComponent.Position;
			}

			foreach (var entityId in enemyFilter)
			{
				ref var enemyComponent = ref enemyComponents.Get(entityId);
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);

				var position = rigidbodyComponent.Position;
				var movementVector = (playerPosition - position).normalized;

				var collisionAreaConfig = _sharedData.EnemyConfigProvider.Get(enemyComponent.ConfigId).CollisionAreaConfig;

				GetNeighbors(ref _neighborsEntityIds, position, collisionAreaConfig);

				var separateVector = SeparateWithNeighbors(ref _neighborsEntityIds, positionComponents, position, entityId);

				movementComponent.Vector = CombineMovementDirection(separateVector, movementVector).normalized;

				_neighborsEntityIds.Clear();
			}

#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}

		private static Vector2 CombineMovementDirection(Vector2 separateVector, Vector2 movementVector)
		{
			return separateVector*10f + movementVector*0.1f;
		}

		private Vector2 SeparateWithNeighbors(
			ref List<int> neighborsEntityIds,
			EcsPool<PositionComponent> positionComponents,
			Vector2 position,
			int excludeEntityId)
		{
			var separateVector = Vector2.zero;
			for (var i = 0; i < neighborsEntityIds.Count; i++)
			{
				var neighborEntityId = neighborsEntityIds[i];

				if (neighborEntityId == excludeEntityId)
				{
					continue;
				}

				ref var neighborPositionComponent = ref positionComponents.Get(neighborEntityId);
				var neighborPosition = neighborPositionComponent.Position;

				var oppositeDirection = position - neighborPosition;
				if (oppositeDirection.sqrMagnitude > 0)
				{
					separateVector += oppositeDirection;
				}

				Debug.DrawLine(position, neighborPosition, Color.red, Time.deltaTime);
			}
			return separateVector;
		}

		private void GetNeighbors(ref List<int> cachedNeighborsEntityIds, Vector2 position, ICollisionAreaConfig collisionAreaConfig)
		{
			_overlapExecutor.Overlap(
				_query,
				position, 
				collisionAreaConfig,
				ref cachedNeighborsEntityIds,
				_neighborsLayer);
		}

		private bool IsInFieldOfView(Vector2 position, Vector2 targetPosition)
		{
			return Vector2.Angle(position, targetPosition) < FIELD_OF_VIEW;
		}
	}
}