using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemBasePresenter : Presenter<IInventoryItemView, IInventoryItemContext>
	{
		private readonly IInventoryModel _inventoryModel;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private CompositeDisposable _subscriptions = new CompositeDisposable();
		public InventoryItemBasePresenter(
			IInventoryItemView view, 
			IInventoryItemContext context,
			IMetaGameModelProvider metaGameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, context)
		{
			_inventoryModel = metaGameModelProvider.Get().ProfileModel.InventoryModel;
			_buttonPresenterFactory = buttonPresenterFactory;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

			UpdateView();
			AddButtonPresenter();
			AddSubscription();
		}

		public override void Dispose()
		{
			_subscriptions?.Dispose();
			base.Dispose();
		}

		private void AddSubscription()
		{
			var itemModel = _inventoryModel.GetItemModel(Context.UserItemId.Value);
			Context.IsEquipped.Subscribe(state => View.SetEquippedState(state)).AddTo(_subscriptions);
			Context.MergeState.Subscribe(state => View.SetMergeState(state)).AddTo(_subscriptions);
			itemModel.DetailId.Subscribe(state => UpdateView()).AddTo(_subscriptions);
			itemModel.ItemId.Subscribe(state => UpdateView()).AddTo(_subscriptions);
		}

		private void UpdateView()
		{
			var itemModel = _inventoryModel.GetItemModel(Context.UserItemId.Value);
			var config = _inventoryItemsConfigProvider.Get(itemModel.ItemId.Value);
			var level = config.GetDetails(itemModel.DetailId.Value).Level;
			
			View.SetBackgroundGradientMaterial(Context.BackgroundGradientMaterial);
			View.SetPanelsColor(Context.MainColor);
			View.SetItemIcon(Context.Icon);
			View.SetItemSlotIcon(Context.SlotIcon);
			View.SetPanelText($"Lv.{level}");
			View.SetEquippedState(Context.IsEquipped.Value);
			View.SetMergeState(Context.MergeState.Value);
		}
		
		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(Context.ClickCallback)));
			
			Presenters.Add(presenter);
		}

		public class Factory : PlaceholderFactory<IInventoryItemView, IInventoryItemContext, InventoryItemBasePresenter>
		{
		}
	}
}