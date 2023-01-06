using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class RigidbodyMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>().Inc<PositionComponent>().Inc<RigidbodyComponent>().End();
			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);

				positionComponent.Position = positionComponent.Position + movementComponent.Vector * Time.deltaTime;
				
				movementComponents.Del(entityId);
			}
		}
	}
}