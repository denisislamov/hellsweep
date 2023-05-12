using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxesView : View, IShopLootboxesView
	{
		[SerializeField] 
		private ShopLootboxView _commonLootboxView;
		
		[SerializeField] 
		private ShopLootboxView _uncommonLootboxView;
		
		[SerializeField] 
		private ShopLootboxView _rareLootboxView;
		
		[SerializeField] 
		private ShopLootboxView _legendaryLootboxView;
		
		public IShopLootboxView CommonLootboxView => _commonLootboxView;
		public IShopLootboxView UncommonLootboxView => _uncommonLootboxView;
		public IShopLootboxView RareLootboxView => _rareLootboxView;
		public IShopLootboxView LegendaryLootboxView => _legendaryLootboxView;
	}
}