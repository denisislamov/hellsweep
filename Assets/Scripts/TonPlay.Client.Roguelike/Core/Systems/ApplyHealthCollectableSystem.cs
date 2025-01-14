using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyHealthCollectableSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyHealthCollectableComponent>()
						.Inc<HealthComponent>()
						.Exc<DeadComponent>()
						.End();
			var applyPool = world.GetPool<ApplyHealthCollectableComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var changeHealthPool = world.GetPool<ChangeHealthEvent>();

			foreach (var entityId in filter)
			{
				ref var apply = ref applyPool.Get(entityId);
				
				if (apply.CollectableEntityIds.Count <= 0)
				{
					continue;
				}
				
				ref var changeHealth = ref changeHealthPool.AddOrGet(entityId);
				changeHealth.DifferenceValue += apply.Value;

				foreach (var collectableEntityId in apply.CollectableEntityIds)
				{
					destroyPool.Add(collectableEntityId);
				}

				apply.CollectableEntityIds.Clear();
				apply.Value = 0f;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}