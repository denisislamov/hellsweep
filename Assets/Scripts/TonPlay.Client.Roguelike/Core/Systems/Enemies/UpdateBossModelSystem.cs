using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class UpdateBossModelSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			var bossFilter = world.Filter<BossEnemy>().Inc<HealthComponent>().End();
			var miniBossFilter = world.Filter<MiniBossEnemy>().Inc<HealthComponent>().End();
			var bossExists = bossFilter.GetEntitiesCount() > 0 || miniBossFilter.GetEntitiesCount() > 0;
			
			var bossModel = sharedData.GameModel.BossModel;
			var bossData = bossModel.ToData();

			bossData.Exists = bossExists;
			
			if (!bossExists)
			{
				bossModel.Update(bossData);
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return;
			}

			var healthPool = world.GetPool<HealthComponent>();

			if (bossFilter.GetEntitiesCount() > 0)
			{
				foreach (var entityId in bossFilter)
				{
					ref var health = ref healthPool.Get(entityId);

					bossData.Health = health.CurrentHealth;
					bossData.MaxHealth = health.MaxHealth;
				}
			} 
			else if (miniBossFilter.GetEntitiesCount() > 0)
			{
				foreach (var entityId in miniBossFilter)
				{
					ref var health = ref healthPool.Get(entityId);

					bossData.Health = health.CurrentHealth;
					bossData.MaxHealth = health.MaxHealth;
				}
			}
			
			bossModel.Update(bossData);
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}