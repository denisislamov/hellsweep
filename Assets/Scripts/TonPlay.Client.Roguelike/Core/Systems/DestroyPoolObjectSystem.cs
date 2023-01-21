using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class DestroyPoolObjectSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
 #region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyComponent>().Inc<ViewPoolObjectComponent>().End();
			var pool = world.GetPool<ViewPoolObjectComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				component.ViewPoolObject.Release();
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}