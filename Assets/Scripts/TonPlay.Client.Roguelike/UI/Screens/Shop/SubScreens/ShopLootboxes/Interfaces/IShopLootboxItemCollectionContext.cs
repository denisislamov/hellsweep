using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces
{
	public interface IShopLootboxItemCollectionContext : IScreenContext
	{
		IReadOnlyList<IInventoryItemModel> Items { get; }
	}
}