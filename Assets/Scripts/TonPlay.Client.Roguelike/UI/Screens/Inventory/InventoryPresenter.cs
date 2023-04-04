using System;
using System.Linq;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	internal class InventoryPresenter : Presenter<IInventoryView, IInventoryScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ProfileBarPresenter.Factory _profileBarPresenterFactory;
		private readonly NavigationMenuPresenter.Factory _navigationMenuPresenterFactory;
		private readonly InventoryItemCollectionPresenter.Factory _inventoryItemCollectionPresenter;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		private bool _launchingMatch;
		private ReactiveProperty<bool> _playButtonLockState;

		public InventoryPresenter(
			IInventoryView view,
			IInventoryScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
			InventoryItemCollectionPresenter.Factory inventoryItemCollectionPresenter,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_navigationMenuPresenterFactory = navigationMenuPresenterFactory;
			_inventoryItemCollectionPresenter = inventoryItemCollectionPresenter;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;

			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddItemCollectionPresenter();
			AddUserProfileUpdateScheduler();
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

			data.BalanceData.Gold = userBalanceResponse.coin;
			data.BalanceData.Energy = userBalanceResponse.energy;

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
				new NavigationMenuContext(NavigationMenuTabName.Inventory)
				{
					Screen = Context.Screen
				});
			
			Presenters.Add(presenter);
		}
		
		private void AddItemCollectionPresenter()
		{
			var items = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Items.Select(_ => _.Id.Value).ToList();
			var presenter = _inventoryItemCollectionPresenter.Create(
				View.ItemCollectionView, 
				new InventoryItemCollectionContext(items));
			
			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IInventoryView, IInventoryScreenContext, InventoryPresenter>
		{
		}
	}
}