using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerLevelUpgradeSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int MAX_SKILLS_TO_CHOICE = 3;

		public const int MAX_DEFENCE_SKILLS = 6;
		public const int MAX_UTILITY_SKILLS = 6;

		private readonly IUIService _uiService;

		private IGameModel _gameModel;
		private ISkillConfigProvider _skillsConfigProvider;
		private EcsWorld _world;
		private IPlayersLevelsConfigProvider _playersLevelsConfigProvider;

		public PlayerLevelUpgradeSystem(IUIService uiService)
		{
			_uiService = uiService;
		}

		public void Init(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_gameModel = sharedData.GameModel;
			_skillsConfigProvider = sharedData.SkillsConfigProvider;
			_playersLevelsConfigProvider = sharedData.PlayersLevelsConfigProvider;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<PlayerComponent>()
						.Inc<LevelUpgradeEvent>()
						.Inc<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<OpenedUIComponent>()
						.End();

			foreach (var entityId in filter)
			{
				if (!TryGenerateSkillsToUpgradeAndShowScreen(entityId))
				{
					break;
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		private bool TryGenerateSkillsToUpgradeAndShowScreen(int entityId)
		{
			var levelUpgradeEventPool = _world.GetPool<LevelUpgradeEvent>();
			var openedUiPool = _world.GetPool<OpenedUIComponent>();

			ref var upgradeEvent = ref levelUpgradeEventPool.Get(entityId);
			upgradeEvent.Used = true;

			if (upgradeEvent.Count <= 0)
			{
				return false;
			}

			upgradeEvent.Count--;


			var skillsToUpgrade = GenerateSkillsToUpgrade(entityId);

			if (skillsToUpgrade.Count == 0)
			{
				Debug.LogWarning("There's no skills to upgrade.");
				return false;
			}

			openedUiPool.Add(entityId);

			SetGamePauseState(true);

			ShowSkillChoiceScreen(skillsToUpgrade, entityId);

			return true;
		}

		private IReadOnlyList<SkillName> GenerateSkillsToUpgrade(int entityId)
		{
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skills = _skillsConfigProvider.All;
			ref var entitySkillsComponent = ref skillsPool.Get(entityId);
			var entitySkills = entitySkillsComponent.Levels;
			var defenceSkillsCount = entitySkills.Count(_ => _skillsConfigProvider.Get(_.Key).SkillType == SkillType.Defence || _skillsConfigProvider.Get(_.Key).SkillType == SkillType.UltimateDefence);
			var utilitySkillsCount = entitySkills.Count(_ => _skillsConfigProvider.Get(_.Key).SkillType == SkillType.Utility);

			var availableSkillsEnumerable = skills
										   .Where(config => (!entitySkills.ContainsKey(config.SkillName) && !IsLimitedBySkillType(config, defenceSkillsCount, utilitySkillsCount))
															|| (entitySkills.ContainsKey(config.SkillName) && entitySkills[config.SkillName] < config.MaxLevel))
										   .Where(config => !config.ExcludeFromInitialDrop ||
															(entitySkills.ContainsKey(config.SkillName) && entitySkills[config.SkillName] > 0));

			var availableSkills = availableSkillsEnumerable.ToList();

			var choiceAmount = Math.Min(MAX_SKILLS_TO_CHOICE, availableSkills.Count);

			var result = new SkillName[choiceAmount];

			for (var i = 0; i < choiceAmount; i++)
			{
				var randomSkillIndex = Random.Range(0, availableSkills.Count);
				result[i] = availableSkills[randomSkillIndex].SkillName;
				availableSkills.RemoveAt(randomSkillIndex);
			}

			return result;
		}

		private bool IsLimitedBySkillType(ISkillConfig config, int defenceSkillsCount, int utilitySkillsCount)
		{
			switch (config.SkillType)
			{
				case SkillType.Defence when defenceSkillsCount == MAX_DEFENCE_SKILLS:
				case SkillType.UltimateDefence when defenceSkillsCount == MAX_DEFENCE_SKILLS:
				case SkillType.Utility when utilitySkillsCount == MAX_UTILITY_SKILLS:
					return true;
				default:
					return false;
			}
		}

		private void ShowSkillChoiceScreen(
			IEnumerable<SkillName> skillsToUpgrade,
			int entityId)
		{
			_uiService.Open<SkillChoiceScreen, ISkillChoiceScreenContext>(
				new SkillChoiceScreenContext(skillsToUpgrade, skillName =>
				{
					SkillChosenHandler(skillName, entityId);
				}));
		}

		private void SkillChosenHandler(SkillName updatedSkill, int entityId)
		{
			var openedUiPool = _world.GetPool<OpenedUIComponent>();
			var levelUpgradeEventPool = _world.GetPool<LevelUpgradeEvent>();

			openedUiPool.Del(entityId);

			UpgradeChosenSkill(updatedSkill, entityId);
			SetGamePauseState(false);
			SyncPlayerSkillsModel(entityId);

			ref var upgradeEvent = ref levelUpgradeEventPool.Get(entityId);
			if (upgradeEvent.Count > 0)
			{
				TryGenerateSkillsToUpgradeAndShowScreen(entityId);
			}
		}

		private void SyncPlayerSkillsModel(int entityId)
		{
			var playerPool = _world.GetPool<PlayerComponent>();
			if (playerPool.Has(entityId))
			{
				var skillsPool = _world.GetPool<SkillsComponent>();
				ref var skills = ref skillsPool.Get(entityId);

				var skillsData = _gameModel.PlayerModel.SkillsModel.ToData();

				foreach (var kvp in skills.Levels)
				{
					if (!skillsData.SkillLevels.ContainsKey(kvp.Key))
					{
						skillsData.SkillLevels.Add(kvp.Key, 0);
					}

					skillsData.SkillLevels[kvp.Key] = kvp.Value;
				}

				_gameModel.PlayerModel.SkillsModel.Update(skillsData);
			}
		}

		private void UpgradeChosenSkill(SkillName updatedSkill, int entityId)
		{
			var skillsPool = _world.GetPool<SkillsComponent>();
			ref var skills = ref skillsPool.Get(entityId);

			if (!skills.Levels.ContainsKey(updatedSkill))
			{
				skills.Levels.Add(updatedSkill, 0);
			}

			skills.Levels[updatedSkill]++;
		}

		private void SetGamePauseState(bool state)
		{
			var gameData = _gameModel.ToData();
			gameData.Paused = state;
			_gameModel.Update(gameData);
		}
	}
}