using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using Time = UnityEngine.Time;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class AccelerationSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world
						.Filter<SpeedComponent>()
						.Inc<AccelerationComponent>()
						.Exc<DestroyComponent>()
						.Exc<DeadComponent>()
						.Exc<InactiveComponent>()
						.End();
			var speedPool = world.GetPool<SpeedComponent>();
			var accelerationPool = world.GetPool<AccelerationComponent>();

			foreach (var entityId in filter)
			{
				ref var speed = ref speedPool.Get(entityId);
				ref var acceleration = ref accelerationPool.Get(entityId);
				
				speed.Speed += acceleration.Acceleration * Time.deltaTime;
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}