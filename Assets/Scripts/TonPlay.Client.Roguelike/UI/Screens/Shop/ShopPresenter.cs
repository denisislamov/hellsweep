using System;
using System.Linq;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Merge;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	internal class ShopPresenter : Presenter<IShopView, IShopScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ProfileBarPresenter.Factory _profileBarPresenterFactory;
		private readonly NavigationMenuPresenter.Factory _navigationMenuPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		private bool _launchingMatch;
		private ReactiveProperty<ShopNavTab> _currentNavTab = new ReactiveProperty<ShopNavTab>();
		private InventoryItemCollectionPresenter _inventoryItemsPresenter;
		private DictionaryExt<ShopNavTab, ReactiveProperty<bool>> _navTabLockProperties;
		private IScreen _currentSubScreen;

		public ShopPresenter(
			IShopView view,
			IShopScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_navigationMenuPresenterFactory = navigationMenuPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;

			_navTabLockProperties = new DictionaryExt<ShopNavTab, ReactiveProperty<bool>>()
			{
				[ShopNavTab.Packs] = new ReactiveProperty<bool>(Context.InitialTab == ShopNavTab.Packs),
				[ShopNavTab.Lootboxes] = new ReactiveProperty<bool>(Context.InitialTab == ShopNavTab.Lootboxes),
				[ShopNavTab.Resources] = new ReactiveProperty<bool>(Context.InitialTab == ShopNavTab.Resources)
			};

			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddUserProfileUpdateScheduler();
			AddNavTabPresenters();
			SetCurrentNavTab(Context.InitialTab);
			AddCurrentNavTabSubscription();
		}
		
		private void AddCurrentNavTabSubscription()
		{
			_currentNavTab.Subscribe(currentTab =>
			{
				if (!(_currentSubScreen is null))
				{
					_uiService.Close(_currentSubScreen, true);
				}

				switch (currentTab)
				{
					case ShopNavTab.Packs:
					{
						_currentSubScreen = _uiService.Open<ShopPacksScreen, ShopPacksScreenContext>(new ShopPacksScreenContext(), true);
						break;
					}
					case ShopNavTab.Lootboxes:
					{
						_currentSubScreen = _uiService.Open<ShopLootboxesScreen, ShopLootboxesScreenContext>(new ShopLootboxesScreenContext(), true);
						break;
					}
					case ShopNavTab.Resources:
					{
						_currentSubScreen = _uiService.Open<ShopResourcesScreen, ShopResourcesScreenContext>(new ShopResourcesScreenContext(), true);
						break;
					}
				}
				
				foreach (var kvp in _navTabLockProperties)
				{
					kvp.Value.SetValueAndForceNotify(kvp.Key == currentTab);
				}
			}).AddTo(_compositeDisposables);
		}

		public override void Show()
		{
			base.Show();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}

		private void AddUserProfileUpdateScheduler()
		{
			Observable
			   .Interval(TimeSpan.FromSeconds(60)) // TODO - move to some config
			   .Subscribe(_ => UpdateUserProfile())
			   .AddTo(_compositeDisposables);
		}

		private async void UpdateUserProfile()
		{
			var userBalanceResponse = await _restApiClient.GetUserBalance();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();

			data.BalanceData.Gold = userBalanceResponse.response.coins;
			data.BalanceData.Energy = userBalanceResponse.response.energy;

			model.Update(data);
		}

		private void AddNestedProfileBarPresenter()
		{
			var presenter = _profileBarPresenterFactory.Create(View.ProfileBarView, new ProfileBarContext());

			Presenters.Add(presenter);
		}

		private void AddNavigationMenuPresenter()
		{
			var presenter = _navigationMenuPresenterFactory.Create(
				View.NavigationMenuView,
				new NavigationMenuContext(NavigationMenuTabName.Shop)
				{
					Screen = Context.Screen
				});

			Presenters.Add(presenter);
		}

		private void AddNavTabPresenters()
		{
			var presenter = CreateNavTabButtonPresenter(View.PacksNavBarButtonView, ShopNavTab.Packs);
			Presenters.Add(presenter);

			presenter = CreateNavTabButtonPresenter(View.LootboxesNavBarButtonView, ShopNavTab.Lootboxes);
			Presenters.Add(presenter);
			
			presenter = CreateNavTabButtonPresenter(View.ResourcesNavBarButtonView, ShopNavTab.Resources);
			Presenters.Add(presenter);
		}
		
		private IButtonPresenter CreateNavTabButtonPresenter(IButtonView view, ShopNavTab navTab) =>
			_buttonPresenterFactory.Create(
				view,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => SetCurrentNavTab(navTab)))
				   .Add(new ReactiveLockButtonContext(_navTabLockProperties[navTab])));

		private void SetCurrentNavTab(ShopNavTab packs)
		{
			_currentNavTab.SetValueAndForceNotify(packs);
		}

		internal class Factory : PlaceholderFactory<IShopView, IShopScreenContext, ShopPresenter>
		{
		}
	}
}