using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemConfig
	{
		string Id { get; }
		
		string Name { get; }
		
		RarityName Rarity { get; }
		
		SlotName SlotName { get; }
		
		AttributeName AttributeName { get; }

		IReadOnlyDictionary<ushort, IInventoryItemDetailConfig> Details { get; }
	}
}