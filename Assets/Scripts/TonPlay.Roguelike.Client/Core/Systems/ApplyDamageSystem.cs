using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine.Profiling;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ApplyDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<HealthComponent>().Inc<ApplyDamageComponent>().Exc<DeadComponent>().End();
			var healthComponents = world.GetPool<HealthComponent>();
			var applyDamageComponents = world.GetPool<ApplyDamageComponent>();
			var deadPool = world.GetPool<DeadComponent>();
			var viewPool = world.GetPool<ViewProviderComponent>();

			foreach (var entityId in filter)
			{
				ref var healthComponent = ref healthComponents.Get(entityId);
				ref var applyDamage = ref applyDamageComponents.Get(entityId);
				healthComponent.CurrentHealth -= applyDamage.Damage;

				if (healthComponent.CurrentHealth <= 0)
				{
					deadPool.Add(entityId);

					if (viewPool.Has(entityId))
					{
						ref var view = ref viewPool.Get(entityId);
						view.View.SetActive(false);
					}
				}

				applyDamageComponents.Del(entityId);
			}
#region Profiling End
			Profiler.EndSample();
#endregion 
		}
	}
}