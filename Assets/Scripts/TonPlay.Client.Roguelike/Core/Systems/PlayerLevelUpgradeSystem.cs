using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerLevelUpgradeSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int MAX_SKILLS_TO_CHOICE = 3;

		private readonly IUIService _uiService;

		private IGameModel _gameModel;
		private ISkillConfigProvider _skillsConfigProvider;
		private EcsWorld _world;

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
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world
						.Filter<PlayerComponent>()
						.Inc<LevelUpgradeEvent>()
						.Inc<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<OpenedUIComponent>()
						.End();

			var levelUpgradeEventPool = world.GetPool<LevelUpgradeEvent>();
			var openedUiPool = world.GetPool<OpenedUIComponent>();

			foreach (var entityId in filter)
			{
				ref var upgradeEvent = ref levelUpgradeEventPool.Get(entityId);
				upgradeEvent.Used = true;

				while (upgradeEvent.Count > 0)
				{
					upgradeEvent.Count--;

					var skillsToUpgrade = GenerateSkillsToUpgrade();

					if (skillsToUpgrade.Count == 0)
					{
						Debug.LogWarning("There's no skills to upgrade.");
						break;
					}
					
					openedUiPool.Add(entityId);

					SetGamePauseState(true);

					ShowSkillChoiceScreen(skillsToUpgrade, entityId);
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}

		private IReadOnlyList<SkillName> GenerateSkillsToUpgrade()
		{
			var skills = _skillsConfigProvider.All;
			var skillsModel = _gameModel.PlayerModel.SkillsModel;

			var availableSkills = skills
								 .Where(config => !skillsModel.SkillLevels.ContainsKey(config.SkillName)
												  || skillsModel.SkillLevels[config.SkillName] < config.MaxLevel)
								 .ToList();

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
			openedUiPool.Del(entityId);
			
			UpgradeChosenSkill(updatedSkill, entityId);
			SetGamePauseState(false);
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