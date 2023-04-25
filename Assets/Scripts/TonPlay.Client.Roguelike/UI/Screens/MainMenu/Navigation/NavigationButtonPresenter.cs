using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationButtonPresenter : Presenter<INavigationButtonView, INavigationButtonContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		public NavigationButtonPresenter(
			INavigationButtonView view,
			INavigationButtonContext context,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;

			AddNestedButtonsPresenter();
			SubscribeToActiveNavigationTab();
			InitView();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void InitView()
		{
			View.SetActiveState(IsTabActive());
		}
		
		private void SubscribeToActiveNavigationTab()
		{
			Context.CurrentActiveNavigationTabName.Subscribe(tabName => View.SetActiveState(IsTabActive())).AddTo(_compositeDisposables);
		}
		
		private bool IsTabActive() => Context.TabName == Context.CurrentActiveNavigationTabName.Value;

		private void AddNestedButtonsPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(Context.OnClickCallback)));

			Presenters.Add(presenter);
		}

		public class Factory : PlaceholderFactory<INavigationButtonView, INavigationButtonContext, NavigationButtonPresenter>
		{
		}
	}
}