using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopView : View, IShopView
	{
		[SerializeField] 
		private ProfileBarView _profileBarView;
		
		[SerializeField] 
		private NavigationMenuView _navigationMenuView;
		
		[SerializeField] 
		private ButtonView _packsNavBarButtonView;
		
		[SerializeField] 
		private ButtonView _lootboxesNavBarButtonView;
		
		[SerializeField] 
		private ButtonView _resourcesNavBarButtonView;

		public IProfileBarView ProfileBarView => _profileBarView;
		
		public INavigationMenuView NavigationMenuView => _navigationMenuView;
		
		public IButtonView PacksNavBarButtonView => _packsNavBarButtonView;
		
		public IButtonView LootboxesNavBarButtonView => _lootboxesNavBarButtonView;
		
		public IButtonView ResourcesNavBarButtonView => _resourcesNavBarButtonView;
	}
}