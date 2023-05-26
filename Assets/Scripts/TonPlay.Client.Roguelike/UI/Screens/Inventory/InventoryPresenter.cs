using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Merge;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

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
		private readonly IPlayerConfigProvider _playerConfigProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		private readonly DictionaryExt<string, IInventoryItemState> _itemStates = new DictionaryExt<string, IInventoryItemState>();
		private readonly ReactiveProperty<string> _sortButtonText = new ReactiveProperty<string>();

		private readonly ReactiveProperty<InventorySortType> _currentSortType = new ReactiveProperty<InventorySortType>(InventorySortType.Rarity);
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
			IPlayerConfigProvider playerConfigProvider,
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
			_playerConfigProvider = playerConfigProvider;
			_restApiClient = restApiClient;

			InitView();
			RefreshItems();
			UpdateSkinView();
			RefreshAttributes();
			AddNestedProfileBarPresenter();
			AddNavigationMenuPresenter();
			AddSlotPresenters();
			AddUserProfileUpdateScheduler();
			AddSubscriptionToSlotsToRefreshAttributes();
			AddToggleSortPanelButtonPresenter();
			AddSortButtonsPresenters();
			AddSubscriptionToCurrentSortType();
			AddMergeButtonPresenter();
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
			RefreshSortButtonText();
		}

		private void AddSubscriptionToCurrentSortType()
		{
			_currentSortType
			   .SkipLatestValueOnSubscribe()
			   .Subscribe(sortType =>
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

				if (slot.ItemId?.Value is null) continue;

				var itemModel = GetItemModel(slot.ItemId.Value);

				if (itemModel is null) continue;

				var itemConfig = _inventoryItemsConfigProvider.Get(itemModel.ItemId.Value);
				var detailConfig = itemConfig.GetDetails(itemModel.DetailId.Value);

				switch (itemConfig.AttributeName)
				{
					case AttributeName.ATTACK:
						damage += detailConfig.Value;
						break;
					case AttributeName.HEALTH:
						health += detailConfig.Value;
						break;
					case AttributeName.ARMOR:
						armor += detailConfig.Value;
						break;
				}
			}

			View.SetArmorValueText(armor.ToString());
			View.SetAttackValueText(damage.ToString());
			View.SetHealthValueText(health.ToString());
		}

		private IInventoryItemModel GetItemModel(string itemId)
			=> _metaGameModelProvider.Get().ProfileModel.InventoryModel.Items.FirstOrDefault(_ => _.Id.Value == itemId);

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

				var itemConfig = _inventoryItemsConfigProvider.Get(items[i].Model.ItemId.Value);

				items[i].SetEquippedState(slots[itemConfig.SlotName].ItemId.Value == items[i].Model.Id.Value);
			}

			var presenter = _inventoryItemCollectionPresenter.Create(
				View.ItemCollectionView,
				new InventoryItemCollectionContext(items, ItemClickHandler));

			Presenters.Add(presenter);

			_inventoryItemsPresenter = presenter;
		}

		private void UpdateSkinView()
		{
			var slotUserItemId = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots[SlotName.WEAPON].ItemId?.Value;
			var itemId = string.IsNullOrEmpty(slotUserItemId)
				? "bae7a647-359a-4bb5-ae6b-7181a616cf7f"
				: _metaGameModelProvider.Get().ProfileModel.InventoryModel.GetItemModel(slotUserItemId)?.ItemId?.Value;

			var skinConfig = _playerConfigProvider.GetSkin(_playerConfigProvider.Get().SkinId);

			if (string.IsNullOrEmpty(itemId))
			{
				itemId = "bae7a647-359a-4bb5-ae6b-7181a616cf7f";
			}

			for (var i = 0; i < View.SkinRoot.childCount; i++)
			{
				Object.Destroy(View.SkinRoot.GetChild(i).gameObject);
			}

			var prefab = skinConfig.GetInventorySpriteForWeaponItemId(itemId);

			if (prefab == null)
			{
				itemId = "bae7a647-359a-4bb5-ae6b-7181a616cf7f";
				prefab = skinConfig.GetInventorySpriteForWeaponItemId(itemId);
			}

			Object.Instantiate(prefab, View.SkinRoot);
		}

		private int SortItemsByCurrentSortType(IInventoryItemState x, IInventoryItemState y)
		{
			var xConfig = _inventoryItemsConfigProvider.Get(x.Model.ItemId.Value);
			var yConfig = _inventoryItemsConfigProvider.Get(y.Model.ItemId.Value);

			switch (_currentSortType.Value)
			{
				case InventorySortType.Level:
				{
					return xConfig.GetDetails(x.Model.DetailId.Value).Level > yConfig.GetDetails(y.Model.DetailId.Value).Level ? -1 : 1;
				}
				case InventorySortType.Rarity:
				{
					return xConfig.Rarity > yConfig.Rarity ? -1 : 1;
				}
				case InventorySortType.Slot:
				{
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

		private void ItemClickHandler(IInventoryItemModel item)
		{
			_uiService.Open<InventoryItemUpgradeScreen, InventoryItemUpgradeScreenContext>(
				new InventoryItemUpgradeScreenContext(item, EquipItem, false));
		}

		private void SlotClickHandler(ISlotModel slotModel)
		{
			if (slotModel.ItemId?.Value is null)
			{
				return;
			}

			var itemModel = GetItemModel(slotModel.ItemId.Value);

			_uiService.Open<InventoryItemUpgradeScreen, InventoryItemUpgradeScreenContext>(
				new InventoryItemUpgradeScreenContext(itemModel, UnequipItem, true));
		}

		private async void UnequipItem(IInventoryItemModel item)
		{
			var itemConfig = _inventoryItemsConfigProvider.Get(item.ItemId.Value);
			var requiredSlot = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots[itemConfig.SlotName];

			var response = await _restApiClient.DeleteItem(requiredSlot.Id.Value);
			if (response != null && response.successful)
			{
				SetSlotItemEquippedState(requiredSlot, false);
				ClearSlot(requiredSlot);
				UpdateSkinView();
			}
		}

		private async void EquipItem(IInventoryItemModel item)
		{
			var itemConfig = _inventoryItemsConfigProvider.Get(item.ItemId.Value);
			var requiredSlot = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots[itemConfig.SlotName];

			var response = await _restApiClient.PutItem(
				new ItemPutBody()
				{
					userItemId = item.Id.Value,
					slotId = requiredSlot.Id.Value
				});

			if (response != null)
			{
				SetSlotItemEquippedState(requiredSlot, false);
				AddItemToSlotModel(item, requiredSlot);
				SetSlotItemEquippedState(requiredSlot, true);
				UpdateSkinView();
			}
		}

		private static void ClearSlot(ISlotModel slotModel)
		{
			var data = slotModel.ToData();
			data.ItemId = null;
			slotModel.Update(data);
		}

		private void SetSlotItemEquippedState(ISlotModel requiredSlot, bool state)
		{
			if (requiredSlot.ItemId?.Value is null)
			{
				return;
			}

			_itemStates[requiredSlot.ItemId.Value].SetEquippedState(state);
		}

		private static void AddItemToSlotModel(IInventoryItemModel item, ISlotModel requiredSlot)
		{
			var requiredSlotData = requiredSlot.ToData();
			requiredSlotData.ItemId = item.Id.Value;
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

		private void AddMergeButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.MergeButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnMergeButtonClickHandler)));

			Presenters.Add(presenter);
		}

		private void OnMergeButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
			_uiService.Open<MergeScreen, IMergeScreenContext>(new MergeScreenContext());
		}

		internal class Factory : PlaceholderFactory<IInventoryView, IInventoryScreenContext, InventoryPresenter>
		{
		}
	}
}