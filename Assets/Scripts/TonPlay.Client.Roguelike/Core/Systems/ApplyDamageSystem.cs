using Leopotam.EcsLite;
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
			var sharedData = systems.GetShared<ISharedData>();
			var world = systems.GetWorld();

			var filter = world
						.Filter<HealthComponent>()
						.Inc<ApplyDamageComponent>()
						.Exc<DeadComponent>()
						.End();

			var healthComponents = world.GetPool<HealthComponent>();
			var applyDamageComponents = world.GetPool<ApplyDamageComponent>();
			var deadPool = world.GetPool<DeadComponent>();
			var enemyPool = world.GetPool<EnemyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			var gameModelData = sharedData.GameModel.ToData();

			foreach (var entityId in filter)
			{
				ref var healthComponent = ref healthComponents.Get(entityId);
				ref var applyDamage = ref applyDamageComponents.Get(entityId);
				healthComponent.CurrentHealth -= applyDamage.Damage;

				if (healthComponent.CurrentHealth <= 0)
				{
					deadPool.Add(entityId);

					if (enemyPool.Has(entityId))
					{
						ref var position = ref positionPool.Get(entityId);
						ref var enemy = ref enemyPool.Get(entityId);

						ref var diedEvent = ref world.NewEntity().Add<EnemyDiedEvent>();
						diedEvent.Position = position.Position;
						diedEvent.EnemyConfig = sharedData.EnemyConfigProvider.Get(enemy.ConfigId);

						destroyPool.Add(entityId);

						gameModelData.DeadEnemies++;
					}
				}

				applyDamageComponents.Del(entityId);
			}

			sharedData.GameModel.Update(gameModelData);
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}