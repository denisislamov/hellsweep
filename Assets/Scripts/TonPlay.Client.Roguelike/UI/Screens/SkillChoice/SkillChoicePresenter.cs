using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	internal class SkillChoicePresenter : Presenter<ISkillChoiceView, ISkillChoiceScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly SkillChoiceCollectionPresenter.Factory _collectionPresenterFactory;
		private readonly IUIService _uiService;

		public SkillChoicePresenter(
			ISkillChoiceView view, 
			ISkillChoiceScreenContext context,
			IGameModelProvider gameModelProvider,
			SkillChoiceCollectionPresenter.Factory collectionPresenterFactory,
			IUIService uiService) 
			: base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_collectionPresenterFactory = collectionPresenterFactory;
			_uiService = uiService;

			AddCollectionPresenter();
		}
		
		private void AddCollectionPresenter()
		{
			var presenter = _collectionPresenterFactory.Create(
				View.CollectionView,
				new SkillChoiceCollectionContext(Context.SkillsToUpgrade, SkillClickedHandler));
			
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