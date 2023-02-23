using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyNonPoolGameObjectSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyComponent>().Inc<GameNonPoolObject>().Exc<ViewPoolObjectComponent>().End();
			var pool = world.GetPool<GameNonPoolObject>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				Object.Destroy(component.GameObject);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}