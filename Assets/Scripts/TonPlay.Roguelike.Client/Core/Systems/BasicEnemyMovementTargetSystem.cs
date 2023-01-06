using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class BasicEnemyMovementTargetSystem : IEcsInitSystem, IEcsRunSystem
	{
		private List<int> _neighbors;
		
		private const float AVOID_RADIUS = 0.6f;
		private const float FIELD_OF_VIEW = 120f;

		public void Init(EcsSystems systems)
		{
			_neighbors = new List<int>(16);
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var enemyFilter = world.Filter<EnemyComponent>().Inc<PositionComponent>().Inc<SpeedComponent>().End();
			var playerFilter = world.Filter<PlayerComponent>().Inc<PositionComponent>().End();
			
			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();
			var speedComponents = world.GetPool<SpeedComponent>();

			var playerPosition = Vector2.zero;
			foreach (var entityId in playerFilter) {
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);
				playerPosition = rigidbodyComponent.Position;
			}

			foreach (var entityId in enemyFilter) {
				ref var movementComponent = ref movementComponents.Add(entityId);
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);
				ref var speedComponent = ref speedComponents.Get(entityId);
				
				var position = rigidbodyComponent.Position;
				var movementVector = (playerPosition - position).normalized;

				GetNeighbors(ref _neighbors, world, AVOID_RADIUS, position, entityId);

				var separateVector = SeparateWithNeighbors(_neighbors, positionComponents, position);

				movementComponent.Vector = CombineMovementDirection(separateVector, movementVector).normalized * speedComponent.Speed;
				
				_neighbors.Clear();
			}
		}
		
		private static Vector2 CombineMovementDirection(Vector2 separateVector, Vector2 movementVector)
		{
			return separateVector * 0.8f + movementVector * 0.2f;
		}
		
		private Vector2 SeparateWithNeighbors(List<int> neighbors, EcsPool<PositionComponent> positionComponents, Vector2 position)
		{
			var separateVector = Vector2.zero;
			for (var i = 0; i < neighbors.Count; i++)
			{
				var neighborEntityId = neighbors[i];
				ref var neighborPositionComponent = ref positionComponents.Get(neighborEntityId);
				var neighborPosition = neighborPositionComponent.Position;

				if (IsInFieldOfView(position, neighborPosition))
				{
					var oppositeDirection = position - neighborPosition;
					if (oppositeDirection.sqrMagnitude > 0)
					{
						separateVector += oppositeDirection;
					}
				}
			}
			return separateVector;
		}

		private void GetNeighbors(ref List<int> cachedNeighbors, EcsWorld world, float radius, Vector2 position, int excludeEntityId)
		{
			var enemyFilter = world.Filter<EnemyComponent>().Inc<PositionComponent>().End();
			var positionComponents = world.GetPool<PositionComponent>();

			foreach (var entityId in enemyFilter)
			{
				if (entityId == excludeEntityId)
				{
					continue;
				}

				ref var positionComponent = ref positionComponents.Get(entityId);

				if (Vector2.Distance(positionComponent.Position, position) > radius)
				{
					continue;
				}
				
				cachedNeighbors.Add(entityId);
			}
		}

		private bool IsInFieldOfView(Vector2 position, Vector2 targetPosition)
		{
			return Vector2.Angle(position, targetPosition) < FIELD_OF_VIEW;
		}
	}
}