using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources;
using UniRx;
using Zenject;

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
		private ReactiveProperty<ShopNavTabName> _currentNavTab = new ReactiveProperty<ShopNavTabName>();
		private InventoryItemCollectionPresenter _inventoryItemsPresenter;
		private DictionaryExt<ShopNavTabName, ReactiveProperty<bool>> _navTabLockProperties;
		private IShopEmbeddedScreenStorage _embeddedScreenStorage;

		public ShopPresenter(
			IShopView view,
			IShopScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			IShopEmbeddedScreenStorage embeddedScreenStorage)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_navigationMenuPresenterFactory = navigationMenuPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_embeddedScreenStorage = embeddedScreenStorage;

			_navTabLockProperties = new DictionaryExt<ShopNavTabName, ReactiveProperty<bool>>()
			{
				[ShopNavTabName.Packs] = new ReactiveProperty<bool>(Context.InitialTabName == ShopNavTabName.Packs),
				[ShopNavTabName.Lootboxes] = new ReactiveProperty<bool>(Context.InitialTabName == ShopNavTabName.Lootboxes),
				[ShopNavTabName.Resources] = new ReactiveProperty<bool>(Context.InitialTabName == ShopNavTabName.Resources)
			};

			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddUserProfileUpdateScheduler();
			AddNavTabPresenters();
			SetCurrentNavTab(Context.InitialTabName);
			AddCurrentNavTabSubscription();
		}
		
		private void AddCurrentNavTabSubscription()
		{
			_currentNavTab.Subscribe(currentTab =>
			{
				if (!(_embeddedScreenStorage.Current is null))
				{
					_uiService.Close(_embeddedScreenStorage.Current, true);
				}

				switch (currentTab)
				{
					case ShopNavTabName.Packs:
					{
						_embeddedScreenStorage.Set(
							_uiService.Open<ShopPacksScreen, ShopPacksScreenContext>(new ShopPacksScreenContext(), true));
						break;
					}
					case ShopNavTabName.Lootboxes:
					{
						_embeddedScreenStorage.Set(
							_uiService.Open<ShopLootboxesScreen, ShopLootboxesScreenContext>(new ShopLootboxesScreenContext(), true));
						break;
					}
					case ShopNavTabName.Resources:
					{
						_embeddedScreenStorage.Set(
							_uiService.Open<ShopResourcesScreen, ShopResourcesScreenContext>(new ShopResourcesScreenContext(), true));
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
			var presenter = CreateNavTabButtonPresenter(View.PacksNavBarButtonView, ShopNavTabName.Packs);
			Presenters.Add(presenter);

			presenter = CreateNavTabButtonPresenter(View.LootboxesNavBarButtonView, ShopNavTabName.Lootboxes);
			Presenters.Add(presenter);
			
			presenter = CreateNavTabButtonPresenter(View.ResourcesNavBarButtonView, ShopNavTabName.Resources);
			Presenters.Add(presenter);
		}
		
		private IButtonPresenter CreateNavTabButtonPresenter(IButtonView view, ShopNavTabName navTabName) =>
			_buttonPresenterFactory.Create(
				view,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => SetCurrentNavTab(navTabName)))
				   .Add(new ReactiveLockButtonContext(_navTabLockProperties[navTabName])));

		private void SetCurrentNavTab(ShopNavTabName packs)
		{
			_currentNavTab.SetValueAndForceNotify(packs);
		}

		internal class Factory : PlaceholderFactory<IShopView, IShopScreenContext, ShopPresenter>
		{
		}
	}
}