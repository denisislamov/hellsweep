using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationMenuView : View, INavigationMenuView
	{
		[SerializeField]
		private NavigationButtonView _mainMenuButtonView;
		
		[SerializeField]
		private NavigationButtonView _inventoryButtonView;

		public INavigationButtonView MainMenuButtonView => _mainMenuButtonView;
		public INavigationButtonView InventoryButtonView => _inventoryButtonView;
	}
}