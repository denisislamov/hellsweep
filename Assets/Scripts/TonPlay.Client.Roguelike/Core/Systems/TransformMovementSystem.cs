using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TransformMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>()
							  .Inc<PositionComponent>()
							  .Inc<TransformComponent>()
							  .Exc<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			var movementComponents = world.GetPool<MovementComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var lerpComponents = world.GetPool<LerpTransformComponent>();
			var speedComponents = world.GetPool<SpeedComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);

				var speed = speedComponents.Has(entityId) ? speedComponents.Get(entityId).Speed : 1f;
				
				var targetPosition = positionComponent.Position + movementComponent.Direction * speed * Time.deltaTime;

				if (lerpComponents.Has(entityId))
				{
					ref var lerpTransformComponent = ref lerpComponents.Get(entityId);
					positionComponent.Position = Vector2.Lerp(positionComponent.Position, targetPosition, lerpTransformComponent.ValueToInterpolate);
				}
				else
				{
					positionComponent.Position = targetPosition;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}