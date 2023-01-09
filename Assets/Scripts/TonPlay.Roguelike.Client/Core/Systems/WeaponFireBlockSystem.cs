using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class WeaponFireBlockSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
 #region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<WeaponFireBlockComponent>().Exc<DeadComponent>().End();
			var pool = world.GetPool<WeaponFireBlockComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				component.TimeLeft -= Time.deltaTime;

				if (component.TimeLeft <= 0)
				{
					pool.Del(entityId);
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}