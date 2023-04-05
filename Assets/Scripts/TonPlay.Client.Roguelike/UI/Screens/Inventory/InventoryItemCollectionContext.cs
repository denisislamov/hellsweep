using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemCollectionContext : ScreenContext, IInventoryItemCollectionContext
	{
		public IReadOnlyList<IInventoryItemModel> Items { get; }

		public InventoryItemCollectionContext(IReadOnlyList<IInventoryItemModel> items)
		{
			Items = items;
		}
	}
}