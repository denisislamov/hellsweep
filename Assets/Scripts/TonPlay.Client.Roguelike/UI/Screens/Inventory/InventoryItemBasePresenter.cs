using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemBasePresenter : Presenter<IInventoryItemView, IInventoryItemContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private IDisposable _subscription;
		public InventoryItemBasePresenter(
			IInventoryItemView view, 
			IInventoryItemContext context,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			
			InitView();
			AddButtonPresenter();
			AddSubscription();
		}

		public override void Dispose()
		{
			_subscription?.Dispose();
			base.Dispose();
		}

		private void AddSubscription()
		{
			_subscription = Context.IsEquipped.Subscribe(state => View.SetEquippedState(state));
		}

		private void InitView()
		{
			View.SetBackgroundGradientMaterial(Context.BackgroundGradientMaterial);
			View.SetPanelsColor(Context.MainColor);
			View.SetItemIcon(Context.Icon);
			View.SetItemSlotIcon(Context.SlotIcon);
			View.SetPanelText($"Lv.{Context.Level}");
			View.SetEquippedState(Context.IsEquipped.Value);
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