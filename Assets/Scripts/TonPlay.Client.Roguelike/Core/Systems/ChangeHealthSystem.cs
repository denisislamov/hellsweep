using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ChangeHealthSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			
			var filter = world
						.Filter<ChangeHealthEvent>()
						.Inc<HealthComponent>()
						.Exc<DeadComponent>()
						.End();
			
			var healthPool = world.GetPool<HealthComponent>();
			var changeHealthPool = world.GetPool<ChangeHealthEvent>();
			var deadPool = world.GetPool<DeadComponent>();
			var enemyPool = world.GetPool<EnemyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			var gameModelData = sharedData.GameModel.ToData();
			
			foreach (var entityId in filter)
			{
				ref var changeHealth = ref changeHealthPool.Get(entityId);
				ref var health = ref healthPool.Get(entityId);
				
				health.CurrentHealth += changeHealth.DifferenceValue;
				
				changeHealthPool.Del(entityId);
				
				if (health.CurrentHealth <= 0)
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
			}
			
			sharedData.GameModel.Update(gameModelData);
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}