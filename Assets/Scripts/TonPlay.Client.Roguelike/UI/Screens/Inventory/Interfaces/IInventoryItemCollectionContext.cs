using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemCollectionContext : IScreenContext
	{
		IReadOnlyList<IInventoryItemState> Items { get; }
		
		Action<IInventoryItemModel> ItemClickCallback { get; }
	}
}