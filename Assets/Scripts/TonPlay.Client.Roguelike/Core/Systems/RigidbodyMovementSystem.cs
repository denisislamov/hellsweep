using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RigidbodyMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
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
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();

			foreach (var entityId in filter)
			{
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);

				var speed = speedComponents.Has(entityId) ? speedComponents.Get(entityId).Speed : 1f;

				if (movementComponent.Direction.sqrMagnitude > 0)
				{
					rigidbodyComponent.Rigidbody.MovePosition(rigidbodyComponent.Rigidbody.position + movementComponent.Direction*(speed*Time.deltaTime));
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}