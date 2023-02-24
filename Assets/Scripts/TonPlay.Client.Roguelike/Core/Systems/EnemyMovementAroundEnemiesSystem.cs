using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EnemyMovementAroundEnemiesSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private ISharedData _sharedData;
		private List<int> _neighborsEntityIds;
		private int _neighborsLayer;

		private KDQuery _query = new KDQuery();

		public EnemyMovementAroundEnemiesSystem(IOverlapExecutor overlapExecutor)
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
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var overlapParams = OverlapParams.Create(world);

			var enemyFilter = overlapParams.CreateDefaultFilterMask()
										   .Inc<EnemyComponent>()
										   .Inc<PositionComponent>()
										   .Inc<SpeedComponent>()
										   .Inc<MovementComponent>()
										   .End();
			var playerFilter = world.Filter<PlayerComponent>().Inc<PositionComponent>().End();

			var enemyComponents = world.GetPool<EnemyComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();
			var collisionComponents = world.GetPool<CollisionComponent>();

			overlapParams.SetFilter(enemyFilter);
			overlapParams.Build();

			var playerPosition = Vector2.zero;
			foreach (var entityId in playerFilter)
			{
				ref var positionComponent = ref positionComponents.Get(entityId);
				playerPosition = positionComponent.Position;
			}

			foreach (var entityId in enemyFilter)
			{
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);
				ref var collisionComponent = ref collisionComponents.Get(entityId);

				var position = rigidbodyComponent.Position;

				var collisionAreaConfig = collisionComponent.CollisionAreaConfig;

				GetNeighbors(ref _neighborsEntityIds, position, collisionAreaConfig, overlapParams);

				var separateVector = SeparateWithNeighbors(ref _neighborsEntityIds, positionComponents, position, entityId);

				movementComponent.Direction = CombineMovementDirection(separateVector, movementComponent.Direction);
				movementComponent.Direction.Normalize();

				_neighborsEntityIds.Clear();
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private static Vector2 CombineMovementDirection(Vector2 separateVector, Vector2 movementVector)
		{
			return separateVector*0.75f + movementVector*0.25f;
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
			}
			return separateVector;
		}

		private void GetNeighbors(ref List<int> cachedNeighborsEntityIds, Vector2 position, ICollisionAreaConfig collisionAreaConfig, IOverlapParams overlapParams)
		{
			_overlapExecutor.Overlap(
				_query,
				position,
				collisionAreaConfig,
				ref cachedNeighborsEntityIds,
				_neighborsLayer,
				overlapParams);
		}
	}
}