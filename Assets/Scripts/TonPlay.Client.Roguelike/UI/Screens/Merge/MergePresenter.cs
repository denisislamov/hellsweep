using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Player.Configs;
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
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using UniRx;
using UnityEngine;
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

            AddMergeButtonPresenter();
            
            InitView();
            RefreshItems();
        }

        public override void Show()
        {
            base.Show();
            View.Show();
            View.SortPanelView.Hide();
            View.MergeButtonView.Hide();
        }

        public override void Hide()
        {
            base.Hide();
            View.Hide();
        }
        
        private void InitView()
        {
            SetCurrentSortType(InventorySortType.Rarity);
            UpdateView();
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
            AddSlotPresenter(0, View.Slots[0]);
            AddSlotPresenter(1, View.Slots[1]); 
            AddSlotPresenter(2, View.Slots[2]);
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
            foreach (var kvp in _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots)
            {
                // kvp.Value.Updated.Subscribe((unit) => RefreshAttributes()).AddTo(_compositeDisposables);
            }
        }
        
        private void RefreshAttributes()
        {
            // TODO - add attributes
        }
        
        private void AddSlotPresenter(int index, IInventorySlotView view)
        {
            var slotsModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots;
            var slotModel = slotsModel[index];
            
            var presenter = _inventorySlotPresenterFactory.Create(
                view,
                new InventorySlotContext(slotModel, () => SlotClickHandler(index, slotModel)));

            Presenters.Add(presenter);
        }
        
        private void AddSettingsButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.GameSettingsButtonView,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(OnSettingsButtonClickHandler)));

            Presenters.Add(presenter);
        }
        
        private void SlotClickHandler(int index, ISlotModel slotModel)
        {
            Debug.LogFormat("SlotClickHandler {0}", slotModel.SlotName);
            
            if (slotModel.ItemId?.Value == string.Empty)
            {
                return;
            }
            
            // var itemModel = GetItemModel(slotModel.ItemId.Value);
            
            var mergingSlots = _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots;
            int i = 0;

            for (; i < mergingSlots.Count; i++)
            {
                if (mergingSlots[i].ItemId?.Value == string.Empty)
                {
                    break;
                }
            }
            i--;
            
            SetSlotItemInMergingState(mergingSlots[i], MergeStates.NONE);
            
            var slotData = mergingSlots[i].ToData();
            slotData.ItemId = string.Empty;
            mergingSlots[i].Update(slotData);
            
            UpdateView();
            RefreshItems();
        }
        
        private void SetSlotItemInMergingState(ISlotModel requiredSlot, MergeStates state)
        {
            if (requiredSlot.ItemId?.Value == string.Empty)
            {
                return;
            }
            
            _itemStates[requiredSlot.ItemId.Value].SetMergeState(state);
        }
        
        private void InMergeItem(ISlotModel requiredSlot, IInventoryItemModel item)
        {
            SetSlotItemInMergingState(requiredSlot, MergeStates.NONE);
            AddItemToSlotModel(item, requiredSlot);
            SetSlotItemInMergingState(requiredSlot, MergeStates.IN_MERGE);
            
            UpdateView();
            RefreshItems();
        }
        
        private static void AddItemToSlotModel(IInventoryItemModel item, ISlotModel requiredSlot)
        {
            var requiredSlotData = requiredSlot.ToData();
            requiredSlotData.ItemId = item.Id.Value;
            requiredSlotData.Id = item.ItemId.Value;
            
            requiredSlot.Update(requiredSlotData);
        }
        
        private void AddInventoryButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.InventoryButtonView,
                new CompositeButtonContext()
                    .Add(new ClickableButtonContext(OnInventoryButtonClickHandler)));

            Presenters.Add(presenter);
        }

        private void AddMergeButtonPresenter()
        {
            var presenter = _buttonPresenterFactory.Create(View.MergeButtonView,
                new CompositeButtonContext().Add(new ClickableButtonContext(OnMergeButtonClickHandler)));
            
            Presenters.Add(presenter);
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
			
            var mergeSlots = inventory.MergeSlots;

            for (var i = 0; i < items.Count; i++)
            {
                _itemStates.Add(items[i].Model.Id.Value, items[i]);
                
                items[i].SetMergeState(MergeStates.NONE);
                
                for (var j = 0; j < mergeSlots.Count; j++)
                {
                    if (mergeSlots[j].ItemId.Value == items[i].Model.Id.Value)
                    {
                        items[i].SetMergeState(MergeStates.IN_MERGE);
                    }
                    else
                    {
                        var mergingItemModel = GetItemModel(mergeSlots[0].ItemId.Value);
                        if (mergingItemModel != null)
                        {
                            var mergingItemConfig = _inventoryItemsConfigProvider.Get(mergingItemModel.ItemId.Value);
                            var itemConfig = _inventoryItemsConfigProvider.Get(items[i].Model.ItemId.Value);

                            if (mergingItemConfig.Name != itemConfig.Name ||
                                mergingItemConfig.Rarity != itemConfig.Rarity)
                            {
                                items[i].SetMergeState(MergeStates.NOT_AVAILABLE);
                            }
                            else
                            {
                                if (items[i].MergingState.Value != MergeStates.IN_MERGE)
                                {
                                    items[i].SetMergeState(MergeStates.AVAILABLE);
                                }
                            }
                        }
                    }
                }
                // items[i].SetEquippedState(slots[itemConfig.SlotName].ItemId.Value == items[i].Model.Id.Value);
            }

            var presenter = _inventoryItemCollectionPresenter.Create(
                View.ItemCollectionView,
                new InventoryItemCollectionContext(items, ItemClickHandler));

            Presenters.Add(presenter);

            _inventoryItemsPresenter = presenter;
        }

        private void ItemClickHandler(IInventoryItemModel item)
        {
            Debug.LogFormat("ItemClickHandler: {0}", item.Id.Value);
            var mergingSlots = _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots;
            var i = 0;

            if (mergingSlots.Count > 0 && mergingSlots[0].ItemId.Value != string.Empty)
            {
                var mergingItemModel = GetItemModel(mergingSlots[0].ItemId.Value);
                if (mergingItemModel == null)
                {
                    return;
                }
                
                var mergingItemConfig = _inventoryItemsConfigProvider.Get(mergingItemModel.ItemId.Value);
                var itemConfig = _inventoryItemsConfigProvider.Get(item.ItemId.Value);

                if (mergingItemConfig.Rarity == RarityName.LEGENDARY)
                {
                    return;
                }
                
                if (mergingItemConfig.Name != itemConfig.Name ||
                    mergingItemConfig.Rarity != itemConfig.Rarity)
                {
                    return;
                }
            }
            
            for (; i < mergingSlots.Count; i++)
            {
                if (_metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots[i].ItemId.Value == string.Empty)
                {
                    break;
                }
                
                if (_metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots[i].ItemId.Value == item.Id.Value)
                {
                    return;
                }
            }
            
            if (i < mergingSlots.Count)
            {
                InMergeItem(mergingSlots[i], item);
            }
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

        private async void OnMergeButtonClickHandler()
        {
            var mergingSlots = _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots;
            var itemMergePostBody = new ItemMergePostBody();
            
            foreach (var mergingSlot in mergingSlots)
            {
                var itemModel = GetItemModel(mergingSlot.ItemId.Value);
                if (itemModel is null)
                {
                    return;
                } 
                
                itemMergePostBody.itemDetailIds.Add(itemModel.DetailId.Value);
            }
            
            var itemMergeResponse = await _restApiClient.PostItemMerge(itemMergePostBody);
            if (itemMergeResponse == null)
            {
                Debug.Log("itemMergeResponse == null");
            }
            else
            {
                Debug.Log("Merging success");
                // UpdateInventoryModel(itemMergeResponse.response, itemMergePostBody);
                
                _inventoryItemsPresenter?.Dispose();
                _itemStates?.Clear();
                
                await UpdateInventoryModel();
                
                // UpdateView();
            }
            
            // TODO - update inventory
        }

        private void UpdateInventoryModel(ItemMergeResponse itemMergeResponse, ItemMergePostBody itemMergePostBody)
        {
            var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
            var inventoryData = inventoryModel.ToData();

            inventoryData.Items.RemoveAll(x => itemMergePostBody.itemDetailIds.Contains(x.DetailId));
            
            foreach (var mergeSlot in inventoryData.MergeSlots)
            {
                mergeSlot.ItemId = string.Empty;
                mergeSlot.SlotName = SlotName.NECK;
                mergeSlot.Id = string.Empty;
            }
            
            inventoryModel.Update(inventoryData);
        }
        
        private async UniTask UpdateInventoryModel()
        {
            var itemsResponseTask = _restApiClient.GetUserItems();
            var slotsResponseTask = _restApiClient.GetUserSlots();
            var inventoryResponseTask = _restApiClient.GetUserInventory();

            var itemsResponse = await itemsResponseTask;
            var slotsResponse = await slotsResponseTask;
            var inventoryResponse = await inventoryResponseTask;

            var metaGameModel = _metaGameModelProvider.Get();
            var model = metaGameModel.ProfileModel.InventoryModel;
            var data = model.ToData();
			
            data.Items.Clear();
            data.Slots.Clear();
            data.MergeSlots.Clear();
            
            for (var i = 0; i < itemsResponse.response.items.Count; i++)
            {
                var itemData = itemsResponse.response.items[i];
                var innerItemConfig = _inventoryItemsConfigProvider.GetInnerItemConfig(itemData.itemId);
				
                data.Items.Add(new InventoryItemData()
                {
                    Id = itemData.id,
                    ItemId = itemData.itemId,
                    DetailId = itemData.itemDetailId,
                });
            }
			
            for (var i = 0; i < slotsResponse.response.items.Count; i++)
            {
                var slotData = slotsResponse.response.items[i];
                var slotName = (SlotName) Enum.Parse(typeof(SlotName), slotData.purpose, true);
				
                data.Slots.Add(slotName, new SlotData()
                {
                    Id = slotData.id,
                    SlotName = slotName,
                    ItemId = slotData.userItemId
                });
            }

            for (var i = 0; i < 3; i++)
            {
                data.MergeSlots.Add(new SlotData()
                {
                    Id = string.Empty,
                    SlotName = SlotName.NECK,
                    ItemId = string.Empty
                });
            }

            data.Blueprints = inventoryResponse.response.blueprints;
            model.Update(data);
            
            AddItemCollectionPresenter();
            UpdateView();
        }
        
        internal class Factory : PlaceholderFactory<IMergeView, IMergeScreenContext, MergePresenter>
        {

        }

        private void UpdateView()
        {
            var mergingSlots = _metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots;
            var i = 0;
            for (; i < mergingSlots.Count; i++)
            {
                if (_metaGameModelProvider.Get().ProfileModel.InventoryModel.MergeSlots[i].ItemId.Value == string.Empty)
                {
                    break;
                }
            }
            
            Debug.LogFormat("i {0}", i);
            
            if (i == 0)
            {
                View.MergeButtonView.Hide();
                
                View.MergedItemView.gameObject.SetActive(false);
                View.GlowImage.gameObject.SetActive(false);
                View.DescriptionPanel.gameObject.SetActive(false);
                View.MergeParticles.gameObject.SetActive(false);
                
                View.SelectItemText.gameObject.SetActive(true);
            }
            else if (i > 0 && i <= mergingSlots.Count)
            {
                View.MergeButtonView.Hide();
                View.SelectItemText.gameObject.SetActive(false);
                View.MergedItemView.gameObject.SetActive(true);
                View.GlowImage.gameObject.SetActive(true);
                View.DescriptionPanel.gameObject.SetActive(true);

                if (!View.MergeParticles.gameObject.activeSelf)
                {
                    View.MergeParticles.gameObject.SetActive(true);
                }

                var itemModel = GetItemModel(mergingSlots[0].ItemId.Value);

                if (itemModel != null)
                {
                    var itemConfig = _inventoryItemsConfigProvider.Get(itemModel.ItemId.Value);
                    var detailConfig = itemConfig.GetDetails(itemModel.DetailId.Value);
                    var presentation = _inventoryItemPresentationProvider.GetItemPresentation(itemModel.ItemId.Value);
                    
                    var rarityValue = itemConfig.Rarity;
                    var name = itemConfig.Name;

                    if (rarityValue == RarityName.LEGENDARY)
                    {
                        return;
                    }
                    
                    rarityValue = itemConfig.Rarity + 1;
                    
                    var maxLevelLabel = "Max Lvl";
                    var attributeNameLabel = itemConfig.AttributeName;


                    var nextRarityMap = _inventoryItemsConfigProvider.GetNextRarityMap();
                    View.SetDescriptionHeaderText(rarityValue + " " + name);
                    View.SetDescriptionInfoText(maxLevelLabel + "\n" +
                                                attributeNameLabel);
                    
                    if (nextRarityMap.ContainsKey(itemModel.ItemId.Value))
                    {
                        var items = nextRarityMap[itemModel.ItemId.Value];
                        var maxLevelValue = items.details[items.details.Count - 1].level;
                        var attributeValue= items.details[detailConfig.Level - 1].value;
                        View.SetDescriptionValuesText(maxLevelValue + "\n" +
                                                      attributeValue);

                    }
                    
                    _inventoryItemPresentationProvider.GetColors(rarityValue, out var mainColor, out var rarityMaterial);
                    View.GlowImage.color = mainColor;

                    var particlesMain = View.MergeParticles.main;
                    particlesMain.startColor = mainColor;
                    // <color=#FF5FAB>Legendary</color> Armor Shirt>(1/3 Items)
                    // Max Lvl\nAttack\nMax Lvl
                    // 20 <color=#55FE5D>> 30</color>
                    // 30 <color=#55FE5D>> 140</color>
                    // 40 <color=#55FE5D>> 60</color>
                    View.SetMergedItemView(presentation.Icon);
                }
            }

            if (i == mergingSlots.Count)
            {
                View.MergeButtonView.Show();
            }

            //UpdateItemCollectionPresenter();
        }
        
        private void UpdateItemCollectionPresenter()
        {
            var inventory = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
            var items = inventory.Items
                .Select(item => (IInventoryItemState)new InventoryItemState(item))
                .ToList();
								 
            var mergeSlots = inventory.MergeSlots;

            for (var i = 0; i < _itemStates.Count; i++)
            {
                items[i].SetMergeState(MergeStates.NONE);
                // _itemStates.Add(items[i].Model.Id.Value, items[i]);
                
                for (var j = 0; j < mergeSlots.Count; j++)
                {
                    if (mergeSlots[j].ItemId.Value == string.Empty)
                    {
                        continue;
                    }

                    if (mergeSlots[j].ItemId.Value == items[i].Model.Id.Value)
                    {
                        items[i].SetMergeState(MergeStates.IN_MERGE);
                    }
                    else
                    {
                        var mergingItemModel = GetItemModel(mergeSlots[0].ItemId.Value);
                        if (mergingItemModel == null)
                        {
                            continue;
                        }
                        
                        var mergingItemConfig = _inventoryItemsConfigProvider.Get(mergingItemModel.ItemId.Value);
                        var itemConfig = _inventoryItemsConfigProvider.Get(items[i].Model.ItemId.Value);
                        
                        if (mergingItemConfig.Name != itemConfig.Name ||
                            mergingItemConfig.Rarity != itemConfig.Rarity)
                        {
                            items[i].SetMergeState(MergeStates.NOT_AVAILABLE);
                            Debug.LogFormat("items[i] {0} MergeStates.NOT_AVAILABLE", items[i].Model.ItemId.Value);
                        }
                        else
                        {
                            items[i].SetMergeState(MergeStates.AVAILABLE);
                        }
                    }
                }
            }
        }
    }
}