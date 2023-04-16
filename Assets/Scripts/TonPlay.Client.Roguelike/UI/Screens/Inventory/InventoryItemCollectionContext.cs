using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemCollectionContext : ScreenContext, IInventoryItemCollectionContext
	{
		public IReadOnlyList<IInventoryItemState> Items { get; }
		public Action<IInventoryItemModel> ItemClickCallback { get; }

		public InventoryItemCollectionContext(IReadOnlyList<IInventoryItemState> items, Action<IInventoryItemModel> itemClickCallback)
		{
			Items = items;
			ItemClickCallback = itemClickCallback;
		}
	}
}