using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Systems;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	internal class SkillChoicePresenter : Presenter<ISkillChoiceView, ISkillChoiceScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly SkillChoiceCollectionPresenter.Factory _collectionPresenterFactory;
		private readonly LevelProgressBarPresenter.Factory _levelProgressBarPresenterFactory;
		private readonly IUIService _uiService;
		private readonly ISkillConfigProvider _skillConfigProvider;
		private ReactiveProperty<float> _fakeExperience = new ReactiveProperty<float>(1f);

		public SkillChoicePresenter(
			ISkillChoiceView view,
			ISkillChoiceScreenContext context,
			IGameModelProvider gameModelProvider,
			SkillChoiceCollectionPresenter.Factory collectionPresenterFactory,
			LevelProgressBarPresenter.Factory levelProgressBarPresenterFactory,
			IUIService uiService,
			ISkillConfigProvider skillConfigProvider)
			: base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_collectionPresenterFactory = collectionPresenterFactory;
			_levelProgressBarPresenterFactory = levelProgressBarPresenterFactory;
			_uiService = uiService;
			_skillConfigProvider = skillConfigProvider;

			AddCollectionPresenter();
			AddNestedLevelProgressBarPresenter();
			InitCurrentSkillViews();
		}

		private void InitCurrentSkillViews()
		{
			var gameModel = _gameModelProvider.Get();

			var defenceSkillsCount = 0;
			var utilitySkillsCount = 0;

			foreach (var kvp in gameModel.PlayerModel.SkillsModel.SkillLevels)
			{
				var skillName = kvp.Key;

				var config = _skillConfigProvider.Get(skillName);

				switch (config.SkillType)
				{
					case SkillType.Defence:
					case SkillType.UltimateDefence:
					{
						var view = View.DefenceCurrentSkillViews[defenceSkillsCount];
						view.SetColor(Color.white);
						view.SetIcon(config.Icon);
						defenceSkillsCount++;
						break;
					}
					case SkillType.Utility:
					{
						var view = View.UtilityCurrentSkillViews[defenceSkillsCount];
						view.SetColor(Color.white);
						view.SetIcon(config.Icon);
						utilitySkillsCount++;
						break;
					}
				}
			}

			for (var i = defenceSkillsCount; i < PlayerLevelUpgradeSystem.MAX_DEFENCE_SKILLS; i++)
			{
				View.DefenceCurrentSkillViews[i].SetColor(new Color(1f, 1f, 1f, 0f));
			}
			for (var i = utilitySkillsCount; i < PlayerLevelUpgradeSystem.MAX_UTILITY_SKILLS; i++)
			{
				View.UtilityCurrentSkillViews[i].SetColor(new Color(1f, 1f, 1f, 0f));
			}
		}

		private void AddCollectionPresenter()
		{
			var presenter = _collectionPresenterFactory.Create(
				View.CollectionView,
				new SkillChoiceCollectionContext(Context.SkillsToUpgrade, SkillClickedHandler));

			Presenters.Add(presenter);
		}

		private void AddNestedLevelProgressBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;

			var presenter = _levelProgressBarPresenterFactory.Create(
				View.LevelProgressBarView,
				new LevelProgressBarContext(playerModel.SkillsModel.Level, _fakeExperience, _fakeExperience));

			Presenters.Add(presenter);
		}

		private void SkillClickedHandler(SkillName skillName)
		{
			_uiService.Close(Context.Screen);

			Context.SkillChosenCallback?.Invoke(skillName);
		}

		internal class Factory : PlaceholderFactory<ISkillChoiceView, ISkillChoiceScreenContext, SkillChoicePresenter>
		{
		}
	}
}