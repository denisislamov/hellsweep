using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventorySortPanelView : IView
	{
		IButtonView RarityButtonView { get; }
		IButtonView SlotButtonView { get; }
		IButtonView LevelButtonView { get; }
		
		void Toggle();
	}
}