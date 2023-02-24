using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyPoolObjectSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyComponent>().Inc<ViewPoolObjectComponent>().Exc<GameNonPoolObject>().End();
			var pool = world.GetPool<ViewPoolObjectComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				component.ViewPoolObject.Release();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}