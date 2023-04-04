using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemPresenter : Presenter<IInventoryItemView, IInventoryItemContext>
	{
		public InventoryItemPresenter(
			IInventoryItemView view, 
			IInventoryItemContext context)
			: base(view, context)
		{
			InitView();
		}
		
		private void InitView()
		{
			View.SetBackgroundGradient(Context.BackgroundGradient);
			View.SetPanelsColor(Context.MainColor);
			View.SetItemIcon(Context.Icon);
			View.SetPanelText(Context.Name);
		}

		public class Factory : PlaceholderFactory<IInventoryItemView, IInventoryItemContext, InventoryItemPresenter>
		{
		}
	}

}