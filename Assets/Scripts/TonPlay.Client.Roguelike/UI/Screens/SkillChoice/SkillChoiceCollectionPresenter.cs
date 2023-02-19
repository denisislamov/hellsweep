using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	internal class SkillChoiceCollectionPresenter : CollectionPresenter<ISkillChoiceItemView, ISkillChoiceCollectionContext>
	{
		private readonly ISkillConfigProvider _skillConfigProvider;
		private readonly IGameModelProvider _gameModelProvider;
		private readonly SkillChoiceItemPresenter.Factory _itemPresenterFactory;

		public SkillChoiceCollectionPresenter(
			ICollectionView<ISkillChoiceItemView> view, 
			ISkillChoiceCollectionContext screenContext, 
			ICollectionItemPool<ISkillChoiceItemView> itemPool,
			ISkillConfigProvider skillConfigProvider,
			IGameModelProvider gameModelProvider,
			SkillChoiceItemPresenter.Factory itemPresenterFactory) 
			: base(view, screenContext, itemPool)
		{
			_skillConfigProvider = skillConfigProvider;
			_gameModelProvider = gameModelProvider;
			_itemPresenterFactory = itemPresenterFactory;

			InitView();
		}

		private void InitView()
		{
			var skills = Context.Skills;

			foreach (var skillName in skills)
			{
				var config = _skillConfigProvider.Get(skillName);
				var presentationConfig = _skillConfigProvider.PresentationConfig;
				var skillsModel = _gameModelProvider.Get().PlayerModel.SkillsModel;
				var currentLevel = GetCurrentSkillLevel(skillsModel, skillName);

				var skillType = config.SkillType;
				var title = $"{config.Title} {GetCurrentSkillTypeIconText(config.SkillType, presentationConfig)}";
				var description = config.GetLevelDescription(currentLevel + 1);
				var icon = config.Icon;
				var maxLevel = config.MaxLevel;
				var color = GetCurrentSkillTypeColor(config.SkillType, presentationConfig);
				var levelIcon = GetCurrentSkillTypeLevelIcon(config.SkillType, presentationConfig);

				var view = Add();

				var context = new SkillChoiceItemContext(
					skillName, 
					skillType, 
					title, 
					description, 
					icon, 
					currentLevel, 
					maxLevel, 
					color,
					levelIcon,
					Context.SkillClickedCallback)
				{
					Screen = Context.Screen
				};

				var presenter = _itemPresenterFactory.Create(view, context);
				Presenters.Add(presenter);
			}
		}

		private static int GetCurrentSkillLevel(ISkillsModel skillsModel, SkillName skillName) => 
			skillsModel.SkillLevels.ContainsKey(skillName) 
				? skillsModel.SkillLevels[skillName] 
				: 0;
		
		private static Sprite GetCurrentSkillTypeLevelIcon(SkillType skillType, ISkillPresentationConfig presentationConfig)
		{
			switch (skillType)
			{
				case SkillType.Defence:
					return presentationConfig.DefenceLevelIcon;
				case SkillType.UltimateDefence:
					return presentationConfig.UltimateDefenceLevelIcon;
				case SkillType.Utility:
					return presentationConfig.UtilityLevelIcon;
			}

			return null;
		}
		
		private static Color GetCurrentSkillTypeColor(SkillType skillType, ISkillPresentationConfig presentationConfig)
		{
			switch (skillType)
			{
				case SkillType.Defence:
					return presentationConfig.DefenceColor;
				case SkillType.UltimateDefence:
					return presentationConfig.UltimateDefenceColor;
				case SkillType.Utility:
					return presentationConfig.UtilityColor;
			}

			return Color.white;
		}
		
		private static string GetCurrentSkillTypeIconText(SkillType skillType, ISkillPresentationConfig presentationConfig)
		{
			switch (skillType)
			{
				case SkillType.Defence:
				case SkillType.UltimateDefence:
					return presentationConfig.DefenceIconText;
				case SkillType.Utility:
					return presentationConfig.UtilityIconText;
			}

			return null;
		}

		internal class Factory : PlaceholderFactory<ISkillChoiceCollectionView, ISkillChoiceCollectionContext, SkillChoiceCollectionPresenter>
		{
		}
	}
}