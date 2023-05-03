using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopView : IView
	{
		IProfileBarView ProfileBarView { get; }
		
		INavigationMenuView NavigationMenuView { get; }
		
		IButtonView PacksNavBarButtonView { get; }
		
		IButtonView LootboxesNavBarButtonView { get; }
		
		IButtonView ResourcesNavBarButtonView { get; }
	}
}