using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	internal class InventoryPresenter : Presenter<IInventoryView, IInventoryScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ProfileBarPresenter.Factory _profileBarPresenterFactory;
		private readonly InventorySlotPresenter.Factory _inventorySlotPresenterFactory;
		private readonly NavigationMenuPresenter.Factory _navigationMenuPresenterFactory;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
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
			InventorySlotPresenter.Factory inventorySlotPresenterFactory,
			NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			InventoryItemCollectionPresenter.Factory inventoryItemCollectionPresenter,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_inventorySlotPresenterFactory = inventorySlotPresenterFactory;
			_navigationMenuPresenterFactory = navigationMenuPresenterFactory;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_inventoryItemCollectionPresenter = inventoryItemCollectionPresenter;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;

			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddItemCollectionPresenter();
			AddSlotPresenters();
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

			data.BalanceData.Gold = userBalanceResponse.response.coin;
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
				new NavigationMenuContext(NavigationMenuTabName.Inventory)
				{
					Screen = Context.Screen
				});
			
			Presenters.Add(presenter);
		}
		
		private void AddItemCollectionPresenter()
		{
			var items = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Items;
			var presenter = _inventoryItemCollectionPresenter.Create(
				View.ItemCollectionView, 
				new InventoryItemCollectionContext(items));
			
			Presenters.Add(presenter);
		}
		
		private void AddSlotPresenters()
		{
			AddSlotPresenter(SlotName.ARMS, View.ArmsSlotView);
			AddSlotPresenter(SlotName.BELT, View.BeltSlotView);
			AddSlotPresenter(SlotName.BODY, View.BodySlotView);
			AddSlotPresenter(SlotName.FEET, View.BootsSlotView);
			AddSlotPresenter(SlotName.WEAPON, View.WeaponSlotView);
			AddSlotPresenter(SlotName.NECK, View.NeckSlotView);
		}
		
		private void AddSlotPresenter(SlotName slotName, IInventorySlotView view)
		{
			var slotsModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots;
			var slotModel = slotsModel[slotName];
			
			var presenter = _inventorySlotPresenterFactory.Create(
				view,
				new InventorySlotContext(slotModel, () => SlotClickHandler(slotModel)));
			
			Presenters.Add(presenter);
		}
		
		private async void SlotClickHandler(ISlotModel slotModel)
		{
			Debug.Log($"Has been clicked '{slotModel.SlotName}' slot with item id: '{slotModel.Item?.DetailId?.Value}'");

			var response = await _restApiClient.DeleteItem(slotModel.Id.Value);
			if (response != null && response.successful)
			{
				var data = slotModel.ToData();
				data.Item = new InventoryItemData();
				slotModel.Update(data);
			}
		}

		internal class Factory : PlaceholderFactory<IInventoryView, IInventoryScreenContext, InventoryPresenter>
		{
		}
	}
}