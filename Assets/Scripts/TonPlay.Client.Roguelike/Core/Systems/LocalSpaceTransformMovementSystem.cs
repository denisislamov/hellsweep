using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class LocalSpaceTransformMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>()
							  .Inc<LocalPositionComponent>()
							  .Inc<TransformComponent>()
							  .Inc<MoveInLocalSpaceOfEntityComponent>()
							  .Exc<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			var movementPool = world.GetPool<MovementComponent>();
			var positionPool = world.GetPool<LocalPositionComponent>();
			var lerpPool = world.GetPool<LerpTransformComponent>();
			var speedPool = world.GetPool<SpeedComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementPool.Get(entityId);
				ref var positionComponent = ref positionPool.Get(entityId);

				var speed = speedPool.Has(entityId) ? speedPool.Get(entityId).Speed : 1f;
				
				var targetPosition = positionComponent.Position + movementComponent.Direction * speed * Time.deltaTime;

				if (lerpPool.Has(entityId))
				{
					ref var lerpTransformComponent = ref lerpPool.Get(entityId);
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