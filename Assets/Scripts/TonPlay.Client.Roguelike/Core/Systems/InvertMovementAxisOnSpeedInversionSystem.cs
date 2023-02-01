using System;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class InvertMovementAxisOnSpeedInversionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world
						.Filter<SpeedComponent>()
						.Inc<MovementComponent>()
						.Inc<InvertMovementAxisOnSpeedInversionComponent>()
						.Exc<DestroyComponent>()
						.Exc<DeadComponent>()
						.Exc<InactiveComponent>()
						.End();
			var speedPool = world.GetPool<SpeedComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var invertPool = world.GetPool<InvertMovementAxisOnSpeedInversionComponent>();

			foreach (var entityId in filter)
			{
				ref var speed = ref speedPool.Get(entityId);
				ref var invert = ref invertPool.Get(entityId);
				ref var movement = ref movementPool.Get(entityId);

				if (Math.Abs(Mathf.Sign(invert.PreviousSpeed) - Mathf.Sign(speed.Speed)) > 0.1f)
				{
					movement.Direction = new Vector2(
						movement.Direction.x * (invert.AxisX ? -1 : 1), 
						movement.Direction.y * (invert.AxisY ? -1 : 1));
				}

				invert.PreviousSpeed = speed.Speed;
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}