using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemCollectionContext : ScreenContext, IInventoryItemCollectionContext
	{
		public List<string> Items { get; }
		
		public InventoryItemCollectionContext(List<string> items)
		{
			Items = items;
		}
	}
}