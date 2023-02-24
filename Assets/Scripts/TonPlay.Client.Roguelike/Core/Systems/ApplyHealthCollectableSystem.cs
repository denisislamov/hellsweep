using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
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
			var healthPool = world.GetPool<HealthComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				ref var apply = ref applyPool.Get(entityId);
				
				if (apply.CollectableEntityIds.Count <= 0)
				{
					continue;
				}
				
				ref var health = ref healthPool.Get(entityId);

				health.CurrentHealth = Mathf.Clamp(health.CurrentHealth + apply.Value, 0f, health.MaxHealth);

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