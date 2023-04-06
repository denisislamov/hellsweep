using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemPresenter : Presenter<IInventoryItemView, IInventoryItemContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		public InventoryItemPresenter(
			IInventoryItemView view, 
			IInventoryItemContext context,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			
			InitView();
			AddButtonPresenter();
		}
		
		private void InitView()
		{
			View.SetBackgroundGradientMaterial(Context.BackgroundGradientMaterial);
			View.SetPanelsColor(Context.MainColor);
			View.SetItemIcon(Context.Icon);
			View.SetItemSlotIcon(Context.SlotIcon);
			View.SetPanelText($"Lv.{Context.Level}");
		}
		
		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(Context.ClickCallback)));
			
			Presenters.Add(presenter);
		}

		public class Factory : PlaceholderFactory<IInventoryItemView, IInventoryItemContext, InventoryItemPresenter>
		{
		}
	}

}