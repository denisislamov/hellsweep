using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxItemCollectionContext : ScreenContext, IShopLootboxItemCollectionContext
	{
		public IReadOnlyList<IInventoryItemModel> Items { get; }

		public ShopLootboxItemCollectionContext(IReadOnlyList<IInventoryItemModel> items)
		{
			Items = items;
		}
	}
}