using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ApplyDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<HealthComponent>().Inc<ApplyDamageComponent>().Exc<DeadComponent>().End();
			var healthComponents = world.GetPool<HealthComponent>();
			var applyDamageComponents = world.GetPool<ApplyDamageComponent>();
			var deadPool = world.GetPool<DeadComponent>();

			foreach (var entityId in filter)
			{
				ref var healthComponent = ref healthComponents.Get(entityId);
				ref var applyDamage = ref applyDamageComponents.Get(entityId);
				healthComponent.CurrentHealth -= applyDamage.Damage;

				if (healthComponent.CurrentHealth <= 0)
				{
					deadPool.Add(entityId);
				}
				
				applyDamageComponents.Del(entityId);
			}
		}
	}
}