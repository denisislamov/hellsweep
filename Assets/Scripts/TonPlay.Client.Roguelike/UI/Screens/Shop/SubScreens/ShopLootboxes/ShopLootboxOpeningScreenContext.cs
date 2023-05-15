using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxOpeningScreenContext : ScreenContext, IShopLootboxOpeningScreenContext
	{
		public IObservable<IReadOnlyList<IInventoryItemModel>> ItemsAsObservable { get; }

		public ShopLootboxOpeningScreenContext(IObservable<IReadOnlyList<IInventoryItemModel>> itemsAsObservable)
		{
			ItemsAsObservable = itemsAsObservable;
		}
	}
}