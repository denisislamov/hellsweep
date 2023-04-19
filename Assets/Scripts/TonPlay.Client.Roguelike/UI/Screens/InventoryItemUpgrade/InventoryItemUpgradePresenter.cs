using System;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
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
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
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
			IMetaGameModelProvider metaGameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, context)
		{
			_uiService = uiService;
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
			var config = _inventoryItemsConfigProvider.Get(Context.Item.DetailId.Value);
			var presentation = _inventoryItemPresentationProvider.GetItemPresentation(Context.Item.DetailId.Value);
			var defaultItemIcon = _inventoryItemPresentationProvider.DefaultItemIcon;
			
			_inventoryItemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradient);

			View.SetDescriptionText(presentation?.Description);
			View.SetTitleText(presentation?.Title);
			View.SetItemIcon(presentation?.Icon ? presentation.Icon : defaultItemIcon);
			View.SetLevelText($"Lv. {Context.Item.Level.Value}");
			View.SetRarityText(config.Rarity.ToString().ToLower().FirstCharToUpper());
			View.SetRarityColor(mainColor);
			View.SetItemBackgroundGradientMaterial(backgroundGradient);
			View.SetAttributeValueText($"+{config.Details[Context.Item.Level.Value].Value}");
			
			_equipButtonLockState.SetValueAndForceNotify(Context.IsEquipped && config.SlotName == SlotName.WEAPON);
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

		internal class Factory : PlaceholderFactory<IInventoryItemUpgradeView, IInventoryItemUpgradeScreenContext, InventoryItemUpgradePresenter>
		{
		}
	}
}