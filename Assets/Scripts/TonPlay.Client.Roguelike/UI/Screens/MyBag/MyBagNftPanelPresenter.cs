using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	internal class MyBagNftPanelPresenter : Presenter<IMyBagNftPanelView, IMyBagNftPanelContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		public MyBagNftPanelPresenter(
			IMyBagNftPanelView view, 
			IMyBagNftPanelContext context,
			IButtonPresenterFactory buttonPresenterFactory) : base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			
			AddCloseButtonPresenter();
		}
		
		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CloseButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(CloseButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void CloseButtonClickHandler()
		{
			Context.ClosePanelAction?.Invoke();
		}

		public class Factory : PlaceholderFactory<IMyBagNftPanelView, IMyBagNftPanelContext, MyBagNftPanelPresenter>
		{
		}
	}
}