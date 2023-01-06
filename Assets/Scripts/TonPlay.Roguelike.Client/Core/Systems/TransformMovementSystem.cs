using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class TransformMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>().Inc<PositionComponent>().Inc<TransformComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var lerpComponents = world.GetPool<LerpTransformComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);
				
				var targetPosition = positionComponent.Position + movementComponent.Vector * Time.deltaTime;

				if (lerpComponents.Has(entityId))
				{
					ref var lerpTransformComponent = ref lerpComponents.Get(entityId);
					positionComponent.Position = Vector2.Lerp(positionComponent.Position, targetPosition, lerpTransformComponent.ValueToInterpolate);
				}
				else
				{
					positionComponent.Position = targetPosition;
				}
				
				movementComponents.Del(entityId);
			}
		}
	}
}