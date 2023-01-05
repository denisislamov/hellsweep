using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class MovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>().Inc<RigidbodyComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);
				rigidbodyComponent.Rigidbody.MovePosition(rigidbodyComponent.Rigidbody.position + movementComponent.Vector * Time.deltaTime);
				
				movementComponents.Del(entityId);
			}
		}
	}
}