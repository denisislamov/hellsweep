using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyHealthCollectableSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
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
				ref var health = ref healthPool.Get(entityId);

				health.CurrentHealth += apply.Value;
				health.CurrentHealth %= health.MaxHealth;

				foreach (var collectableEntityId in apply.CollectableEntityIds)
				{
					destroyPool.Add(collectableEntityId);
				}

				applyPool.Del(entityId);
			}
		}
	}
}