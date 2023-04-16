using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventorySlotPresenter : Presenter<IInventorySlotView, IInventorySlotContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IInventoryItemsConfigProvider _itemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private IDisposable _subscription;

		public InventorySlotPresenter(
			IInventorySlotView view, 
			IInventorySlotContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			IInventoryItemsConfigProvider itemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider) 
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_itemsConfigProvider = itemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

			InitView();
			AddButtonPresenter();
			AddModelSubscription();
		}

		public override void Dispose()
		{
			_subscription?.Dispose();
			base.Dispose();
		}

		private void AddModelSubscription()
		{
			_subscription = Context.SlotModel.Updated.Subscribe((unit) => InitView());
		}

		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(Context.ClickCallback)));
			
			Presenters.Add(presenter);
		}

		private void InitView()
		{
			var itemId = Context.SlotModel.Item?.DetailId?.Value;
			var config = _itemsConfigProvider.Get(itemId);
			var slotIsEmpty = string.IsNullOrWhiteSpace(itemId);

			View.SetEmptyState(slotIsEmpty);

			if (slotIsEmpty || config == null)
			{
				return;
			}
			
			var icon = _inventoryItemPresentationProvider.GetIcon(itemId);
				
			_inventoryItemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var rarityMaterial);

			View.SetIcon(icon);
			View.SetBackgroundMaterial(rarityMaterial);
		}

		public class Factory : PlaceholderFactory<IInventorySlotView, IInventorySlotContext, InventorySlotPresenter>
		{
		}
	}
}