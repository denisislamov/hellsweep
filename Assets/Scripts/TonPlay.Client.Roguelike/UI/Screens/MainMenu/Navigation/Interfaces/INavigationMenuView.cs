using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces
{
	public interface INavigationMenuView : IView
	{
		INavigationButtonView MainMenuButtonView { get; }
		
		INavigationButtonView InventoryButtonView { get; }
		
		INavigationButtonView ShopButtonView { get; }
	}
}