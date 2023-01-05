using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class EnemyMovementTargetSystem : IEcsRunSystem
	{
		private const float SPEED = 1f;
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var enemyFilter = world.Filter<EnemyComponent>().Inc<RigidbodyComponent>().End();
			var playerFilter = world.Filter<PlayerComponent>().Inc<RigidbodyComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();

			var playerPosition = Vector2.zero;
			foreach (var entityId in playerFilter) {
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);
				playerPosition = rigidbodyComponent.Rigidbody.position;
			}

			foreach (var entityId in enemyFilter) {
				ref var movementComponent = ref movementComponents.Add(entityId);
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);
				var movementVector = (playerPosition - rigidbodyComponent.Rigidbody.position).normalized;
				movementComponent.Vector = movementVector * SPEED;
			}
		}
	}
}