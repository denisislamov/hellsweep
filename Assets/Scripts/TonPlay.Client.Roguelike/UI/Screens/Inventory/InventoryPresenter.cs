using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
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
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		private readonly DictionaryExt<string, IInventoryItemState> _itemStates = new DictionaryExt<string, IInventoryItemState>();
		private readonly ReactiveProperty<string> _sortButtonText = new ReactiveProperty<string>();

		private readonly ReactiveProperty<InventorySortType> _currentSortType = new ReactiveProperty<InventorySortType>();
		private readonly ReactiveProperty<bool> _sortBySlotButtonActiveState = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<bool> _sortByLevelButtonActiveState = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<bool> _sortByRarityButtonActiveState = new ReactiveProperty<bool>();

		private IRestApiClient _restApiClient;

		private bool _launchingMatch;
		private ReactiveProperty<bool> _playButtonLockState;
		private InventoryItemCollectionPresenter _inventoryItemsPresenter;

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
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
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
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_restApiClient = restApiClient;

			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddSlotPresenters();
			AddUserProfileUpdateScheduler();
			RefreshAttributes();
			AddSubscriptionToSlotsToRefreshAttributes();
			AddToggleSortPanelButtonPresenter();
			AddSortButtonsPresenters();
			AddSubscriptionToCurrentSortType();
			AddSettingsButtonPresenter();
			AddMergeButtonPresenter();
			InitView();
			RefreshItems();
		}

		public override void Show()
		{
			base.Show();
			
			View.SortPanelView.Hide();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void InitView()
		{
			SetCurrentSortType(InventorySortType.Rarity);
		}
		
		private void AddSubscriptionToCurrentSortType()
		{
			_currentSortType.Subscribe(sortType =>
			{
				RefreshItems();
				RefreshSortButtonsActiveState();
				RefreshSortButtonText();
			}).AddTo(_compositeDisposables);
		}
		
		private void RefreshSortButtonText()
		{
			_sortButtonText.SetValueAndForceNotify(_currentSortType.Value.ToString());
		}

		private void RefreshItems()
		{
			_inventoryItemsPresenter?.Dispose();
			_itemStates?.Clear();
			
			AddItemCollectionPresenter();
		}

		private void AddUserProfileUpdateScheduler()
		{
			Observable
			   .Interval(TimeSpan.FromSeconds(60)) // TODO - move to some config
			   .Subscribe(_ => UpdateUserProfile())
			   .AddTo(_compositeDisposables);
		}

		private void AddSubscriptionToSlotsToRefreshAttributes()
		{
			foreach (var kvp in _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots)
			{
				kvp.Value.Updated.Subscribe((unit) => RefreshAttributes()).AddTo(_compositeDisposables);
			}
		}

		private void RefreshAttributes()
		{
			uint armor = 0;
			uint damage = 0;
			uint health = 0;

			foreach (var kvp in _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots)
			{
				var slot = kvp.Value;

				if (slot.Item?.Id?.Value is null) continue;

				var itemConfig = _inventoryItemsConfigProvider.Get(slot.Item.DetailId.Value);

				switch (itemConfig.AttributeName)
				{
					case AttributeName.ATTACK:
						damage += itemConfig.Details[slot.Item.Level.Value].Value;
						break;
					case AttributeName.HEALTH:
						health += itemConfig.Details[slot.Item.Level.Value].Value;
						break;
					case AttributeName.ARMOR:
						armor += itemConfig.Details[slot.Item.Level.Value].Value;
						break;
				}
			}

			View.SetArmorValueText(armor.ToString());
			View.SetAttackValueText(damage.ToString());
			View.SetHealthValueText(health.ToString());
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
			var inventory = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var items = inventory.Items
								 .Select(item => (IInventoryItemState)new InventoryItemState(item))
								 .ToList();
								 
			items.Sort(SortItemsByCurrentSortType);
			
			var slots = inventory.Slots;

			for (var i = 0; i < items.Count; i++)
			{
				_itemStates.Add(items[i].Model.Id.Value, items[i]);

				var itemConfig = _inventoryItemsConfigProvider.Get(items[i].Model.DetailId.Value);

				items[i].SetEquippedState(slots[itemConfig.SlotName].Item.Id.Value == items[i].Model.Id.Value);
			}

			var presenter = _inventoryItemCollectionPresenter.Create(
				View.ItemCollectionView,
				new InventoryItemCollectionContext(items, ItemClickHandler));

			Presenters.Add(presenter);

			_inventoryItemsPresenter = presenter;
		}
		
		private int SortItemsByCurrentSortType(IInventoryItemState x, IInventoryItemState y)
		{
			switch (_currentSortType.Value)
			{
				case InventorySortType.Level:
				{
					return x.Model.Level.Value > y.Model.Level.Value ? -1 : 1;
				}
				case InventorySortType.Rarity:
				{
					var xConfig = _inventoryItemsConfigProvider.Get(x.Model.DetailId.Value);
					var yConfig = _inventoryItemsConfigProvider.Get(y.Model.DetailId.Value);
					
					return xConfig.Rarity > yConfig.Rarity ? -1 : 1;
				}
				case InventorySortType.Slot:
				{
					var xConfig = _inventoryItemsConfigProvider.Get(x.Model.DetailId.Value);
					var yConfig = _inventoryItemsConfigProvider.Get(y.Model.DetailId.Value);
					
					return xConfig.SlotName > yConfig.SlotName ? -1 : 1;
				}
			}

			throw new NotImplementedException();
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

		private async void ItemClickHandler(IInventoryItemModel item)
		{
			var itemConfig = _inventoryItemsConfigProvider.Get(item.DetailId.Value);
			var requiredSlot = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots[itemConfig.SlotName];

			var response = await _restApiClient.PutItem(
				new ItemPutBody()
				{
					itemDetailId = item.Id.Value,
					slotId = requiredSlot.Id.Value
				});

			if (response != null)
			{
				SetSlotItemEquippedState(requiredSlot, false);
				AddItemToSlotModel(item, requiredSlot);
				SetSlotItemEquippedState(requiredSlot, true);
			}
		}

		private async void SlotClickHandler(ISlotModel slotModel)
		{
			if (slotModel.Item?.DetailId?.Value is null)
			{
				return;
			}

			var response = await _restApiClient.DeleteItem(slotModel.Id.Value);
			if (response != null && response.successful)
			{
				SetSlotItemEquippedState(slotModel, false);
				ClearSlot(slotModel);
			}
		}

		private static void ClearSlot(ISlotModel slotModel)
		{
			var data = slotModel.ToData();
			data.Item = new InventoryItemData();
			slotModel.Update(data);
		}

		private void SetSlotItemEquippedState(ISlotModel requiredSlot, bool state)
		{
			if (requiredSlot.Item?.DetailId?.Value is null || requiredSlot.Item?.Id?.Value is null)
			{
				return;
			}

			_itemStates[requiredSlot.Item.Id.Value].SetEquippedState(state);
		}

		private static void AddItemToSlotModel(IInventoryItemModel item, ISlotModel requiredSlot)
		{
			var requiredSlotData = requiredSlot.ToData();
			requiredSlotData.Item = item.ToData();
			requiredSlot.Update(requiredSlotData);
		}

		private void AddToggleSortPanelButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.SortButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(SortButtonClickHandler))
				   .Add(new ReactiveTextButtonContext(_sortButtonText)));
			
			Presenters.Add(presenter);
		}
		
		private void AddSortButtonsPresenters()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.SortPanelView.LevelButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => SetCurrentSortType(InventorySortType.Level)))
				   .Add(new ReactiveLockButtonContext(_sortByLevelButtonActiveState)));
			
			Presenters.Add(presenter);
			
			presenter = _buttonPresenterFactory.Create(
				View.SortPanelView.RarityButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => SetCurrentSortType(InventorySortType.Rarity)))
				   .Add(new ReactiveLockButtonContext(_sortByRarityButtonActiveState)));
			
			Presenters.Add(presenter);
			
			presenter = _buttonPresenterFactory.Create(
				View.SortPanelView.SlotButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => SetCurrentSortType(InventorySortType.Slot)))
				   .Add(new ReactiveLockButtonContext(_sortBySlotButtonActiveState)));
			
			Presenters.Add(presenter);
		}
		
		private void SetCurrentSortType(InventorySortType sortType)
		{
			_currentSortType.SetValueAndForceNotify(sortType);
		}

		private void RefreshSortButtonsActiveState()
		{
			_sortBySlotButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Slot);
			_sortByLevelButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Level);
			_sortByRarityButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Rarity);
		}

		private void SortButtonClickHandler()
		{
			View.SortPanelView.Toggle();
		}
		
		private void AddSettingsButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.GameSettingsButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnSettingsButtonClickHandler)));

			Presenters.Add(presenter);
		}
		
		private void AddMergeButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.MergeButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnMergeButtonClickHandler)));

			Presenters.Add(presenter);
		}
		
		private void OnSettingsButtonClickHandler()
		{
			_uiService.Open<GameSettingsScreen, IGameSettingsScreenContext>(new GameSettingsScreenContext());
		}

		private void OnMergeButtonClickHandler()
		{
			_uiService.Open<GameSettingsScreen, IGameSettingsScreenContext>(new GameSettingsScreenContext());
		}
		
		internal class Factory : PlaceholderFactory<IInventoryView, IInventoryScreenContext, InventoryPresenter>
		{
		}
	}
}