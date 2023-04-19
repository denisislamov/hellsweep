using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemPresenter : Presenter<IInventoryItemView, IInventoryItemContext>
	{
		private readonly InventoryItemBasePresenter.Factory _inventoryItemBasePresenterFactory;
		
		private IDisposable _subscription;
		public InventoryItemPresenter(
			IInventoryItemView view, 
			IInventoryItemContext context,
			InventoryItemBasePresenter.Factory inventoryItemBasePresenterFactory)
			: base(view, context)
		{
			_inventoryItemBasePresenterFactory = inventoryItemBasePresenterFactory;

			AddBasePresenter();
			AddSubscription();
		}

		public override void Show()
		{
			TryHideIfEquippedOrShow();
		}
		
		private void AddBasePresenter()
		{
			var presenter = _inventoryItemBasePresenterFactory.Create(View, Context);
			
			Presenters.Add(presenter);
		}
		
		private void TryHideIfEquippedOrShow()
		{
			if (!Context.IsEquipped.Value)
			{
				base.Show();
			}
			else
			{
				base.Hide();
			}
		}

		public override void Dispose()
		{
			_subscription?.Dispose();
			base.Dispose();
		}

		private void AddSubscription()
		{
			_subscription = Context.IsEquipped.Subscribe(state => TryHideIfEquippedOrShow());
		}

		public class Factory : PlaceholderFactory<IInventoryItemView, IInventoryItemContext, InventoryItemPresenter>
		{
		}
	}
}