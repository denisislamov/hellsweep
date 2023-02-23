using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class LocalSpaceTransformMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
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
			var applyForcePool = world.GetPool<ApplyForceComponent>();

			foreach (var entityId in filter)
			{
				ref var movementComponent = ref movementPool.Get(entityId);
				ref var positionComponent = ref positionPool.Get(entityId);

				var applyForce = new ApplyForceComponent();

				applyForcePool.TryGet(entityId, ref applyForce);

				var speed = speedPool.Has(entityId) ? speedPool.Get(entityId).Speed : 1f;

				var targetPosition = positionComponent.Position
									 + movementComponent.Direction*speed*Time.deltaTime
									 + applyForce.Force*Time.deltaTime;

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
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}