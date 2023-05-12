using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxContext : ScreenContext, IShopLootboxContext
	{
		public RarityName Rarity { get; }
		
		public ShopLootboxContext(RarityName rarity)
		{
			Rarity = rarity;
		}
	}
}