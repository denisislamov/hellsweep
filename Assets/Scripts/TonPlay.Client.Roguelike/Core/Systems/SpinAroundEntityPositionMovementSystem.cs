using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SpinAroundEntityPositionMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<SpinAroundEntityPositionComponent>()
							  .Inc<PositionComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DestroyComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			var positionPool = world.GetPool<PositionComponent>();
			var speedPool = world.GetPool<SpeedComponent>();
			var spinPool = world.GetPool<SpinAroundEntityPositionComponent>();

			foreach (var entityId in filter)
			{
				ref var spin = ref spinPool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var spinAroundPosition = ref positionPool.Get(spin.SpinAroundEntityId);

				var dir = Vector2.right.Rotate(spin.LastAngle);
				var angleDiff = -speed.Speed*Time.deltaTime*360f%360f%-360f;
				var nextAngle = spin.LastAngle + angleDiff;

				var nextDir = dir.Rotate(angleDiff);

				position.Position = spinAroundPosition.Position +
									nextDir*spin.Radius;

				spin.LastAngle = nextAngle;
			}
		}
	}
}