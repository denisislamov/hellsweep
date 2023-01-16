using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerLevelUpgradeSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IUIService _uiService;

		private IGameModel _gameModel;

		public PlayerLevelUpgradeSystem(IUIService uiService)
		{
			_uiService = uiService;
		}

		public void Init(EcsSystems systems)
		{
			_gameModel = systems.GetShared<ISharedData>().GameModel;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<PlayerComponent>()
						.Inc<LevelUpgradeEvent>()
						.Exc<DeadComponent>()
						.End();
			
			var levelUpgradeEventPool = world.GetPool<LevelUpgradeEvent>();

			foreach (var entityId in filter)
			{
				ref var upgradeEvent = ref levelUpgradeEventPool.Get(entityId);
				upgradeEvent.Used = true;

				while (upgradeEvent.Count > 0)
				{
					SetGamePauseState(true);

					ShowSkillChoiceScreen();
					
					upgradeEvent.Count--;
				}
			}
		}

		private void ShowSkillChoiceScreen()
		{
			_uiService.Open<SkillChoiceScreen, ISkillChoiceScreenContext>(new SkillChoiceScreenContext(SkillChosenHandler));
		}

		private void SkillChosenHandler(SkillName updatedSkill)
		{
			UpgradeChosenSkill(updatedSkill);

			SetGamePauseState(false);
		}

		private void UpgradeChosenSkill(SkillName updatedSkill)
		{
			var playerData = _gameModel.PlayerModel.ToData();

			if (!playerData.SkillsData.SkillLevels.ContainsKey(updatedSkill))
			{
				playerData.SkillsData.SkillLevels.Add(updatedSkill, 0);
			}

			playerData.SkillsData.SkillLevels[updatedSkill]++;
		}

		private void SetGamePauseState(bool state)
		{
			var gameData = _gameModel.ToData();
			gameData.Paused = state;
			_gameModel.Update(gameData);
		}
	}
}