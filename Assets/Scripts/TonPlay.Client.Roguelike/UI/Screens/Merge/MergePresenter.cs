using System;
using System.Linq;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    public class MergePresenter : Presenter<IMergeView, IMergeScreenContext>
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
        
        public MergePresenter(
            IMergeView view,
            IMergeScreenContext context,
            IUIService uiService,
            IButtonPresenterFactory buttonPresenterFactory,
            ProfileBarPresenter.Factory profileBarPresenterFactory,
            InventorySlotPresenter.Factory inventorySlotPresenterFactory,
            NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
            IInventoryItemPresentationProvider inventoryItemPresentationProvider,
            InventoryItemCollectionPresenter.Factory inventoryItemCollectionPresenter,
            IMetaGameModelProvider metaGameModelProvider,
            IInventoryItemsConfigProvider inventoryItemsConfigProvider,
            IRestApiClient restApiClient) : base(view, context)
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
            AddInventoryButtonPresenter();
            
            InitView();
            RefreshItems();
        }

        public override void Show()
        {
            base.Show();
            View.Show();
            View.SortPanelView.Hide();
        }

        public override void Hide()
        {
            base.Hide();
            View.Hide();
        }
        
        private void InitView()
        {
            SetCurrentSortType(InventorySortType.Rarity);
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

        private void AddSlotPresenters()
        {
            // AddSlotPresenter(0, View.Slots[0]);
            // AddSlotPresenter(1, View.Slots[0]);
            // AddSlotPresenter(2, View.Slots[0]);
            // AddSlotPresenter(SlotName.ARMS, View.ArmsSlotView);
            // AddSlotPresenter(SlotName.ARMS, View.ArmsSlotView);
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
            // TODO - add attributes
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
        
        private void AddSettingsButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.GameSettingsButtonView,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(OnSettingsButtonClickHandler)));

            Presenters.Add(presenter);
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
            
            SetSlotItemEquippedState(requiredSlot, false);
        }
        
        private void SetSlotItemEquippedState(ISlotModel requiredSlot, bool state)
        {
            if (requiredSlot.ItemId?.Value is null)
            {
                return;
            }

            _itemStates[requiredSlot.ItemId.Value].SetEquippedState(state);
        }
        
        private void AddInventoryButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.InventoryButtonView,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(OnInventoryButtonClickHandler)));

            Presenters.Add(presenter);
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
        
        private IInventoryItemModel GetItemModel(string itemId) 
            => _metaGameModelProvider.Get().ProfileModel.InventoryModel.Items.FirstOrDefault(_ => _.Id.Value == itemId);
        
        public override void Dispose()
        {
            base.Dispose();
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
        
        private void AddSubscriptionToCurrentSortType()
        {
            _currentSortType.Subscribe(sortType =>
            {
                RefreshItems();
                RefreshSortButtonsActiveState();
                RefreshSortButtonText();
            }).AddTo(_compositeDisposables);
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
        
        private void SortButtonClickHandler()
        {
            View.SortPanelView.Toggle();
        }
        
        private void RefreshItems()
        {
            _inventoryItemsPresenter?.Dispose();
            _itemStates?.Clear();
			
            AddItemCollectionPresenter();
        }
        
        private void RefreshSortButtonsActiveState()
        {
            _sortBySlotButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Slot);
            _sortByLevelButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Level);
            _sortByRarityButtonActiveState.SetValueAndForceNotify(_currentSortType.Value == InventorySortType.Rarity);
        }
        
        private void RefreshSortButtonText()
        {
            _sortButtonText.SetValueAndForceNotify(_currentSortType.Value.ToString());
        }
        
        private void SetCurrentSortType(InventorySortType sortType)
        {
            _currentSortType.SetValueAndForceNotify(sortType);
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

                // items[i].SetEquippedState(slots[itemConfig.SlotName].ItemIds.Value == items[i].Model.Id.Value);
            }

            var presenter = _inventoryItemCollectionPresenter.Create(
                View.ItemCollectionView,
                new InventoryItemCollectionContext(items, ItemClickHandler));

            Presenters.Add(presenter);

            _inventoryItemsPresenter = presenter;
        }

        private void ItemClickHandler(IInventoryItemModel item)
        {
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
        
        private void OnSettingsButtonClickHandler()
        {
            _uiService.Open<GameSettingsScreen, IGameSettingsScreenContext>(new GameSettingsScreenContext());
        }
        
        private void OnInventoryButtonClickHandler()
        {
            _uiService.Close(Context.Screen);
            _uiService.Open<InventoryScreen, IInventoryScreenContext>(new InventoryScreenContext());
        }
        
        internal class Factory : PlaceholderFactory<IMergeView, IMergeScreenContext, MergePresenter>
        {

        }
    }
}