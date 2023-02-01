using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyOnTimerSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
 #region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyOnTimerComponent>().Exc<InactiveComponent>().End();
			var pool = world.GetPool<DestroyOnTimerComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				component.TimeLeft -= Time.deltaTime;

				if (component.TimeLeft <= 0)
				{
					pool.Del(entityId);
					destroyPool.AddOrGet(entityId);
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}