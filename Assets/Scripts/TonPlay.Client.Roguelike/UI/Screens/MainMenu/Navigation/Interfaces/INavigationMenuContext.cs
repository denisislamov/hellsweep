using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces
{
	public interface INavigationMenuContext : IScreenContext
	{
		NavigationMenuTabName InitialTab { get; }
	}
}