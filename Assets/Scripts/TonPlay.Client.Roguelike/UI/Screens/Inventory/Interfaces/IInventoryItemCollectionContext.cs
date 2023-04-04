using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemCollectionContext : IScreenContext
	{
		List<string> Items { get; }
	}
}