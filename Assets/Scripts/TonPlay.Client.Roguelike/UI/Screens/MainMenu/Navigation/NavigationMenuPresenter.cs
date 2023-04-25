using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationMenuPresenter : Presenter<INavigationMenuView, INavigationMenuContext>
	{
		private readonly IUIService _uiService;
		private readonly NavigationButtonPresenter.Factory _navigationButtonPresenterFactory;
		
		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly ReactiveProperty<NavigationMenuTabName> _currentActiveTab = new ReactiveProperty<NavigationMenuTabName>();

		public NavigationMenuPresenter(INavigationMenuView view,
			INavigationMenuContext context,
			IUIService uiService,
			NavigationButtonPresenter.Factory navigationButtonPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_navigationButtonPresenterFactory = navigationButtonPresenterFactory;
			
			_currentActiveTab.SetValueAndForceNotify(Context.InitialTab);
			
			AddMainMenuButtonPresenter();
			AddInventoryButtonPresenter();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void AddMainMenuButtonPresenter()
		{
			var context = new NavigationButtonContext(
				OnMainMenuButtonClickHandler,
				NavigationMenuTabName.MainMenu,
				_currentActiveTab)
			{
				Screen = Context.Screen
			};

			var presenter = _navigationButtonPresenterFactory.Create(View.MainMenuButtonView, context);

			Presenters.Add(presenter);
		}
		
		private void AddInventoryButtonPresenter()
		{
			var context = new NavigationButtonContext(
					OnInventoryButtonClickHandler,
					NavigationMenuTabName.Inventory,
					_currentActiveTab)
			{
				Screen = Context.Screen
			};

			var presenter = _navigationButtonPresenterFactory.Create(View.InventoryButtonView, context);

			Presenters.Add(presenter);
		}
		
		private void OnMainMenuButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
			_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
		}
		
		private void OnInventoryButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
			_uiService.Open<InventoryScreen, IInventoryScreenContext>(new InventoryScreenContext());
		}

		public class Factory : PlaceholderFactory<INavigationMenuView, INavigationMenuContext, NavigationMenuPresenter>
		{
		}
	}
}