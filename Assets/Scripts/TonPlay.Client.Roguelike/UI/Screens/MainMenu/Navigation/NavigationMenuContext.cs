using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationMenuContext : ScreenContext, INavigationMenuContext
	{
		public NavigationMenuTabName InitialTab { get; }

		public NavigationMenuContext(NavigationMenuTabName initialTab)
		{
			InitialTab = initialTab;
		}
	}
}