using System;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using UniRx;
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
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, context)
		{
			_uiService = uiService;
			_restApiClient = restApiClient;
			_metaGameModelProvider = metaGameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

			InitView();
			AddCloseButtonPresenter();
			AddEquipButtonPresenter();
			AddUpgradeButtonPresenter();
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
			View.SetLevelText($"Lv. {detailConfig.Level}");
			View.SetRarityText(config.Rarity.ToString().ToLower().FirstCharToUpper());
			View.SetRarityColor(mainColor);
			View.SetItemBackgroundGradientMaterial(backgroundGradient);
			View.SetAttributeValueText($"+{detailConfig.Value}");
			View.SetGoldPriceText(GetGoldPriceText(priceConfig));
			View.SetBlueprintsPriceText(GetBlueprintsPriceText(priceConfig));
			
			_equipButtonLockState.SetValueAndForceNotify(Context.IsEquipped && config.SlotName == SlotName.WEAPON);
			_upgradeButtonLockState.SetValueAndForceNotify(!CanUpgrade(priceConfig));
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
				   .Add(new ClickableButtonContext(UpgradeButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_upgradeButtonLockState)));
			
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
		
		private void UpgradeButtonClickHandler()
		{
		}

		private void CloseButtonClickHandler()
		{
			ClosePopup();
		}
		
		private void ClosePopup()
		{
			_uiService.Close(Context.Screen);
		}
		
		private bool CanUpgrade(IInventoryItemUpgradePriceConfig priceConfig)
		{
			var model = _metaGameModelProvider.Get();
			var currentCoins = model.ProfileModel.BalanceModel.Gold.Value;
			var currentBlueprints = model.ProfileModel.InventoryModel.Blueprints.Value;

			return currentCoins >= priceConfig.Coins && currentBlueprints >= priceConfig.Blueprints;
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