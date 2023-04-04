using System;
using TonPlay.Client.Common.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces
{
	public interface INavigationButtonContext : IScreenContext
	{
		IReadOnlyReactiveProperty<NavigationMenuTabName> CurrentActiveNavigationTabName { get; }

		NavigationMenuTabName TabName { get; }
		
		Action OnClickCallback { get; }
	}
}