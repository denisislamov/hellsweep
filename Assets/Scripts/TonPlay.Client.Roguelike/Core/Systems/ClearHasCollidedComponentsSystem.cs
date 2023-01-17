using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ClearHasCollidedComponentsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>().End();
			var pool = world.GetPool<HasCollidedComponent>();

			foreach (var entityId in filter)
			{
				pool.Del(entityId);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}