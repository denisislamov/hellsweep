using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine.Profiling;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();

			var filter = world
						.Filter<HealthComponent>()
						.Inc<ApplyDamageComponent>()
						.Exc<DeadComponent>()
						.End();

			var changeHealthPool = world.GetPool<ChangeHealthEvent>();
			var applyDamageComponents = world.GetPool<ApplyDamageComponent>();

			foreach (var entityId in filter)
			{
				ref var changeHealth = ref changeHealthPool.AddOrGet(entityId);
				ref var applyDamage = ref applyDamageComponents.Get(entityId);
				changeHealth.DifferenceValue -= applyDamage.Damage;

				applyDamageComponents.Del(entityId);
			}
		}
	}
}