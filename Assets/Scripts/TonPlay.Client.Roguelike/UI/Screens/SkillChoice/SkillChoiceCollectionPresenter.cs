using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
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
				var skillsModel = _gameModelProvider.Get().PlayerModel.SkillsModel;

				var skillType = config.SkillType;
				var title = config.Title;
				var description = config.Description;
				var icon = config.Icon;
				var currentLevel = GetCurrentSkillLevel(skillsModel, skillName);
				var maxLevel = config.MaxLevel;

				var view = Add();

				var context = new SkillChoiceItemContext(skillName, skillType, title, description, icon, currentLevel, maxLevel, Context.SkillClickedCallback)
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

		internal class Factory : PlaceholderFactory<ISkillChoiceCollectionView, ISkillChoiceCollectionContext, SkillChoiceCollectionPresenter>
		{
		}
	}
}