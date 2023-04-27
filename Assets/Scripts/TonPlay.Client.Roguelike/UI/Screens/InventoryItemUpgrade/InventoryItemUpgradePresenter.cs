using System;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	internal class InventoryItemUpgradePresenter : Presenter<IInventoryItemUpgradeView, IInventoryItemUpgradeScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IRestApiClient _restApiClient;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private readonly InventoryItemGradeDescriptionPresenter.Factory _gradeDescriptionPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly ReactiveProperty<bool> _equipButtonLockState = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<string> _equipButtonText = new ReactiveProperty<string>();

		private readonly ReactiveProperty<bool> _upgradeButtonLockState = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<bool> _maxLevelButtonLockState = new ReactiveProperty<bool>();

		public InventoryItemUpgradePresenter(
			IInventoryItemUpgradeView view,
			IInventoryItemUpgradeScreenContext context,
			IUIService uiService,
			IRestApiClient restApiClient,
			IMetaGameModelProvider metaGameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			InventoryItemGradeDescriptionPresenter.Factory gradeDescriptionPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_restApiClient = restApiClient;
			_metaGameModelProvider = metaGameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_gradeDescriptionPresenterFactory = gradeDescriptionPresenterFactory;

			InitView();
			
			AddCloseButtonPresenter();
			AddEquipButtonPresenter();
			AddUpgradeButtonPresenter();
			AddMaxUpgradeButtonPresenter();
			
			AddGradeDescriptionPresenters();
		}

		private void InitView()
		{
			var config = _inventoryItemsConfigProvider.Get(Context.Item.ItemId.Value);
			var detailConfig = config.GetDetails(Context.Item.DetailId.Value);
			var priceConfig = _inventoryItemsConfigProvider.GetUpgradePrice(detailConfig.Level);
			var presentation = _inventoryItemPresentationProvider.GetItemPresentation(Context.Item.ItemId.Value);
			var defaultItemIcon = _inventoryItemPresentationProvider.DefaultItemIcon;

			_inventoryItemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradient);

			View.SetDescriptionText(presentation?.Description);
			View.SetTitleText(presentation?.Title);
			View.SetItemIcon(presentation?.Icon ? presentation.Icon : defaultItemIcon);
			View.SetRarityText(config.Rarity.ToString().ToLower().FirstCharToUpper());
			View.SetRarityColor(mainColor);
			View.SetItemBackgroundGradientMaterial(backgroundGradient);
			View.SetAttributeIcon(_inventoryItemPresentationProvider.GetItemAttributeIcon(config.AttributeName));

			UpdateAttributeValueText(detailConfig);
			UpdateLevelText(detailConfig);
			UpdateGoldPriceText(priceConfig);
			UpdateBlueprintsPriceText(priceConfig);
			UpdateEquipButtonLockState(config);
			UpdateLevelUpButtonLockState(priceConfig, detailConfig);
			UpdateMaxLevelUpButtonLockState(priceConfig, detailConfig);
		}
		
		private void AddGradeDescriptionPresenters()
		{
			var config = _inventoryItemsConfigProvider.Get(Context.Item.ItemId.Value);
			AddGradeDescriptionPresenter(config.Rarity, RarityName.UNCOMMON, View.UncommonGradeDescriptionView);
			AddGradeDescriptionPresenter(config.Rarity, RarityName.RARE, View.RareGradeDescriptionView);
			AddGradeDescriptionPresenter(config.Rarity, RarityName.LEGENDARY, View.LegendaryGradeDescriptionView);
		
			View.UpdateGradeLayout();
			View.UpdateGradeLayout();
			View.UpdateGradeLayout();
		}
		
		private void AddGradeDescriptionPresenter(
			RarityName userItemRarityName, 
			RarityName gradeDescriptionRarityName, 
			IInventoryItemGradeDescriptionView gradeDescriptionView)
		{
			var innerItemConfig = _inventoryItemsConfigProvider.GetInnerItemConfig(Context.Item.ItemId.Value);
			if (innerItemConfig.GetGradeConfig(gradeDescriptionRarityName) is null)
			{
				gradeDescriptionView.Hide();
				return;
			}

			var presenter = _gradeDescriptionPresenterFactory.Create(
				gradeDescriptionView,
				new InventoryItemGradeDescriptionContext(Context.Item.ItemId.Value, userItemRarityName, gradeDescriptionRarityName));
			
			Presenters.Add(presenter);
		}

		private void UpdateLevelText(IInventoryItemDetailConfig detailConfig)
		{
			View.SetLevelText($"Lv. {detailConfig.Level}");
		}

		private void UpdateAttributeValueText(IInventoryItemDetailConfig detailConfig)
		{
			View.SetAttributeValueText($"+{detailConfig.Value}");
		}

		private void UpdateLevelUpButtonLockState(IInventoryItemUpgradePriceConfig priceConfig, IInventoryItemDetailConfig itemDetailConfig)
		{
			_upgradeButtonLockState.SetValueAndForceNotify(!CanUpgrade(priceConfig, itemDetailConfig));
		}
		
		private void UpdateMaxLevelUpButtonLockState(IInventoryItemUpgradePriceConfig priceConfig, IInventoryItemDetailConfig itemDetailConfig)
		{
			_maxLevelButtonLockState.SetValueAndForceNotify(!CanUpgrade(priceConfig, itemDetailConfig));
		}

		private void UpdateEquipButtonLockState(IInventoryItemConfig config)
		{
			_equipButtonLockState.SetValueAndForceNotify(Context.IsEquipped && config.SlotName == SlotName.WEAPON);
		}

		private void UpdateBlueprintsPriceText(IInventoryItemUpgradePriceConfig priceConfig)
		{
			View.SetBlueprintsPriceText(GetBlueprintsPriceText(priceConfig));
		}

		private void UpdateGoldPriceText(IInventoryItemUpgradePriceConfig priceConfig)
		{
			View.SetGoldPriceText(GetGoldPriceText(priceConfig));
		}

		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CloseButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(CloseButtonClickHandler)));

			Presenters.Add(presenter);
		}

		private void AddEquipButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.EquipButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(EquipButtonClickHandler))
				   .Add(new TextButtonContext(GetEquipButtonText()))
				   .Add(new ReactiveLockButtonContext(_equipButtonLockState)));

			Presenters.Add(presenter);
		}

		private void AddUpgradeButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.UpgradeButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => UpgradeButtonClickHandler(false)))
				   .Add(new ReactiveLockButtonContext(_upgradeButtonLockState)));

			Presenters.Add(presenter);
		}
		
		private void AddMaxUpgradeButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.MaxLevelButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => UpgradeButtonClickHandler(true)))
				   .Add(new ReactiveLockButtonContext(_maxLevelButtonLockState)));

			Presenters.Add(presenter);
		}

		private string GetEquipButtonText()
		{
			return Context.IsEquipped ? "Unequip" : "Equip";
		}

		private void EquipButtonClickHandler()
		{
			Context.EquipButtonCallback?.Invoke(Context.Item);
			ClosePopup();
		}

		private async void UpgradeButtonClickHandler(bool isMax)
		{
			var response = await _restApiClient.PutItemLevelUp(Context.Item.Id.Value, isMax);

			if (!response.successful)
			{
				Debug.LogError($"ErrorCode: {response.error.code}");
				return;
			}

			var config = _inventoryItemsConfigProvider.Get(Context.Item.ItemId.Value);
			var previousDetailConfig = config.GetDetails(Context.Item.DetailId.Value);
			var paidUpgradePrice = _inventoryItemsConfigProvider.GetUpgradePrice(previousDetailConfig.Level);

			var itemModel = Context.Item;
			var itemData = itemModel.ToData();
			itemData.DetailId = response.response.itemDetailId;
			itemModel.Update(itemData);

			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;

			var balanceData = balanceModel.ToData();
			var inventoryData = inventoryModel.ToData();

			balanceData.Gold -= paidUpgradePrice.Coins;
			inventoryData.Blueprints -= paidUpgradePrice.Blueprints;

			balanceModel.Update(balanceData);
			inventoryModel.Update(inventoryData);

			var currentDetailConfig = config.GetDetails(Context.Item.DetailId.Value);
			var currentUpgradePrice = _inventoryItemsConfigProvider.GetUpgradePrice(previousDetailConfig.Level);

			UpdateAttributeValueText(currentDetailConfig);
			UpdateLevelText(currentDetailConfig);
			UpdateGoldPriceText(currentUpgradePrice);
			UpdateBlueprintsPriceText(currentUpgradePrice);
			UpdateEquipButtonLockState(config);
			UpdateLevelUpButtonLockState(currentUpgradePrice, currentDetailConfig);
			UpdateMaxLevelUpButtonLockState(currentUpgradePrice, currentDetailConfig);
		}

		private void CloseButtonClickHandler()
		{
			ClosePopup();
		}

		private void ClosePopup()
		{
			_uiService.Close(Context.Screen);
		}

		private bool CanUpgrade(IInventoryItemUpgradePriceConfig priceConfig, IInventoryItemDetailConfig detailConfig)
		{
			var model = _metaGameModelProvider.Get();
			var currentCoins = model.ProfileModel.BalanceModel.Gold.Value;
			var currentBlueprints = model.ProfileModel.InventoryModel.Blueprints.Value;

			return detailConfig.Next != null && currentCoins >= priceConfig.Coins && currentBlueprints >= priceConfig.Blueprints;
		}

		private string GetGoldPriceText(IInventoryItemUpgradePriceConfig priceConfig)
		{
			var currentCoins = _metaGameModelProvider.Get().ProfileModel.BalanceModel.Gold.Value;
			var enoughCoins = currentCoins >= priceConfig.Coins;

			var currentCoinsText = currentCoins.ConvertToSuffixedFormat(1_000L, 2);
			var requiredCoinsText = priceConfig.Coins.ConvertToSuffixedFormat(1_000L, 2);
			currentCoinsText = enoughCoins ? currentCoinsText : $"<color=red>{currentCoinsText}</color>";

			return $"{currentCoinsText}/{requiredCoinsText}";
		}

		private string GetBlueprintsPriceText(IInventoryItemUpgradePriceConfig priceConfig)
		{
			var currentBlueprints = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Blueprints.Value;
			var enoughBlueprints = currentBlueprints >= priceConfig.Blueprints;

			var requiredBlueprintsText = priceConfig.Blueprints.ConvertToSuffixedFormat(1_000L, 2);
			var currentBlueprintsText = currentBlueprints.ConvertToSuffixedFormat(1_000L, 2);
			currentBlueprintsText = enoughBlueprints ? currentBlueprintsText : $"<color=red>{currentBlueprintsText}</color>";

			return $"{currentBlueprintsText}/{requiredBlueprintsText}";
		}

		internal class Factory : PlaceholderFactory<IInventoryItemUpgradeView, IInventoryItemUpgradeScreenContext, InventoryItemUpgradePresenter>
		{
		}
	}
}