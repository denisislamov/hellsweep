using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyExperienceSystem : IEcsInitSystem, IEcsRunSystem
	{
		private IPlayersLevelsConfigProvider _playersLevelsConfigProvider;

		public void Init(EcsSystems systems)
		{
			_playersLevelsConfigProvider = systems.GetShared<ISharedData>().PlayersLevelsConfigProvider;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyExperienceComponent>()
						.Inc<ExperienceComponent>()
						.Exc<DeadComponent>()
						.End();
			var applyPool = world.GetPool<ApplyExperienceComponent>();
			var expPool = world.GetPool<ExperienceComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var levelUpgradeEventPool = world.GetPool<LevelUpgradeEvent>();

			foreach (var entityId in filter)
			{
				ref var apply = ref applyPool.Get(entityId);
				ref var exp = ref expPool.Get(entityId);

				exp.Value += apply.Value;

				if (exp.Value >= exp.MaxValue)
				{
					exp.Level++;
					exp.Value %= exp.MaxValue;
					exp.MaxValue = _playersLevelsConfigProvider.Get(exp.Level).ExperienceToNextLevel;

					AddUpgradeEvent(levelUpgradeEventPool, entityId);
				}

				foreach (var collectableEntityId in apply.CollectableEntityIds)
				{
					destroyPool.Add(collectableEntityId);
				}

				applyPool.Del(entityId);
			}
		}
		
		private static void AddUpgradeEvent(EcsPool<LevelUpgradeEvent> levelUpgradeEventPool, int entityId)
		{
			if (!levelUpgradeEventPool.Has(entityId))
			{
				ref var upgradeEvent = ref levelUpgradeEventPool.Add(entityId);
				upgradeEvent.Used = false;
				upgradeEvent.Count++;
			}
			else
			{
				ref var upgradeEvent = ref levelUpgradeEventPool.Get(entityId);
				upgradeEvent.Used = false;
				upgradeEvent.Count++;
			}
		}
	}
}