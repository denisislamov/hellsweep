using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class GlobalSpaceTransformMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<MovementComponent>()
							  .Inc<PositionComponent>()
							  .Inc<TransformComponent>()
							  .Exc<MoveInLocalSpaceOfEntityComponent>()
							  .Exc<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			var movementComponents = world.GetPool<MovementComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var lerpComponents = world.GetPool<LerpTransformComponent>();
			var speedComponents = world.GetPool<SpeedComponent>();
			var applyForceComponents = world.GetPool<ApplyForceComponent>();

			foreach (var entityId in filter)
			{
				ref var movementComponent = ref movementComponents.Get(entityId);
				ref var positionComponent = ref positionComponents.Get(entityId);

				var applyForce = new ApplyForceComponent();

				applyForceComponents.TryGet(entityId, ref applyForce);

				var speed = speedComponents.Has(entityId) ? speedComponents.Get(entityId).Speed : 1f;

				var targetPosition = positionComponent.Position
									 + movementComponent.Direction*speed*Time.deltaTime
									 + applyForce.Force*Time.deltaTime;

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
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}