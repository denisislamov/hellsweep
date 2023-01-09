using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class RigidbodyMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>()
							  .Inc<PositionComponent>()
							  .Inc<RigidbodyComponent>()
							  .Exc<InactiveComponent>()
							  .Exc<DeadComponent>()
							  .End();
			var positionComponents = world.GetPool<PositionComponent>();
			var movementComponents = world.GetPool<MovementComponent>();
			var speedComponents = world.GetPool<SpeedComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);

				var speed = speedComponents.Has(entityId) ? speedComponents.Get(entityId).Speed : 1f;

				positionComponent.Position = positionComponent.Position + movementComponent.Vector * (speed * Time.deltaTime);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}