using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearDestroyedOrDeadElementsFromKdTreeSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var pool = world.GetPool<KdTreeElementComponent>();
			var filter = world.Filter<KdTreeElementComponent>().Inc<DestroyComponent>().End();

			foreach (var entityId in filter)
			{
				ref var treeElement = ref pool.Get(entityId);
				treeElement.Storage.RemoveEntity(entityId);
			}

			filter = world.Filter<KdTreeElementComponent>().Inc<DeadComponent>().End();

			foreach (var entityId in filter)
			{
				ref var treeElement = ref pool.Get(entityId);
				treeElement.Storage.RemoveEntity(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}