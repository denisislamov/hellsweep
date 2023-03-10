using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class EnemyMovementAroundEnemiesSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int MAX_NEIGHBORS_CHECK_AMOUNT = 250;
		private const int MAX_FRAMES_CHECK_DELAY = 4;
		
		private readonly IOverlapExecutor _overlapExecutor;

		private ISharedData _sharedData;
		private List<int> _neighborsEntityIds;
		private int _neighborsLayer;
		private int _lastCheckedPart;

		private Stack<int> _debugStack = new Stack<int>();

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

			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();
			var collisionComponents = world.GetPool<CollisionComponent>();

			overlapParams.SetFilter(enemyFilter);
			overlapParams.Build();

			var entitiesCount = enemyFilter.GetEntitiesCount();
			var maxDynamicNeighborsCheckAmount = enemyFilter.GetEntitiesCount() / MAX_FRAMES_CHECK_DELAY;
			var maxNeighborsCheckAmount = maxDynamicNeighborsCheckAmount > MAX_NEIGHBORS_CHECK_AMOUNT ?
				maxDynamicNeighborsCheckAmount:
				MAX_NEIGHBORS_CHECK_AMOUNT;
			
			var maxParts = entitiesCount / maxNeighborsCheckAmount + 1;

			_lastCheckedPart = ++_lastCheckedPart % maxParts;

			var startIndex = _lastCheckedPart * maxNeighborsCheckAmount;
			var lastIndex = (_lastCheckedPart + 1) * maxNeighborsCheckAmount;

			lastIndex = lastIndex > entitiesCount ? entitiesCount : lastIndex;

			var entitiesRaw = enemyFilter.GetRawEntities();

			_debugStack.Clear();
			
			for (var currentIndex = startIndex; currentIndex < lastIndex; currentIndex++)
			{
				var entityId = entitiesRaw[currentIndex];
				
				_debugStack.Push(entityId);
				
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);
				ref var collisionComponent = ref collisionComponents.Get(entityId);

				var position = rigidbodyComponent.Position;

				var collisionAreaConfig = collisionComponent.CollisionArea.Config as ICircleCollisionAreaConfig;

				GetNeighbors(ref _neighborsEntityIds, position, collisionComponent.CollisionArea, overlapParams);

				var separateVector = SeparateWithNeighbors(ref _neighborsEntityIds, positionComponents, position, entityId, collisionAreaConfig?.Radius ?? 0);

				movementComponent.Direction = CombineMovementDirection(separateVector, movementComponent.Direction);
				movementComponent.Direction.Normalize();

				_neighborsEntityIds.Clear();
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private static Vector2 CombineMovementDirection(Vector2 separateVector, Vector2 movementVector)
		{
			return separateVector * 0.66f + movementVector * 0.33f;
		}

		private Vector2 SeparateWithNeighbors(ref List<int> neighborsEntityIds,
											  EcsPool<PositionComponent> positionComponents,
											  Vector2 position,
											  int excludeEntityId, 
											  float radius)
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

		private void GetNeighbors(ref List<int> cachedNeighborsEntityIds, Vector2 position, ICollisionArea collisionArea, IOverlapParams overlapParams)
		{
			_overlapExecutor.Overlap(
				_query,
				position,
				collisionArea,
				ref cachedNeighborsEntityIds,
				_neighborsLayer,
				overlapParams);
		}
	}
}