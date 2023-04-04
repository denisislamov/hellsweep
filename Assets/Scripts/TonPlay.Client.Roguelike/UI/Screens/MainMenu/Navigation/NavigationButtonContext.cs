using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationButtonContext : ScreenContext, INavigationButtonContext
	{
		public IReadOnlyReactiveProperty<NavigationMenuTabName> CurrentActiveNavigationTabName { get; }
		public NavigationMenuTabName TabName { get; }
		public Action OnClickCallback { get; }

		public NavigationButtonContext(
			Action onClickCallback,
			NavigationMenuTabName tabName, 
			IReadOnlyReactiveProperty<NavigationMenuTabName> currentActiveNavigationTabName)
		{
			OnClickCallback = onClickCallback;
			TabName = tabName;
			CurrentActiveNavigationTabName = currentActiveNavigationTabName;
		}
	}
}