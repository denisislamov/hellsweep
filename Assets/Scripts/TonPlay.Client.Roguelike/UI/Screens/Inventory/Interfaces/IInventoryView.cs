using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryView : IView
	{
		IProfileBarView ProfileBarView { get; }
		
		INavigationMenuView NavigationMenuView { get; }
		
		IInventoryItemCollectionView ItemCollectionView { get; }
		
		IInventorySlotView WeaponSlotView { get; }
		
		IInventorySlotView NeckSlotView { get; }
		
		IInventorySlotView ArmsSlotView { get; }
		
		IInventorySlotView BodySlotView { get; }
		
		IInventorySlotView BeltSlotView { get; }

		IInventorySlotView BootsSlotView { get; }
		
		IInventorySortPanelView SortPanelView { get; }
		
		IButtonView SortButtonView { get; }

		void SetAttackValueText(string text);
		
		void SetArmorValueText(string text);
		
		void SetHealthValueText(string text);
	}
}