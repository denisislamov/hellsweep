using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemConfig : IInventoryItemConfig
	{
		private readonly Dictionary<ushort, IInventoryItemDetailConfig> _details;

		public string Id { get; }
		public string Name { get; }
		public RarityName Rarity { get; }
		public SlotName SlotName { get; }
		
		public AttributeName AttributeName { get; }
		public IReadOnlyDictionary<ushort, IInventoryItemDetailConfig> Details => _details;

		public InventoryItemConfig(
			string id, 
			string name, 
			RarityName rarity, 
			SlotName slotName, 
			AttributeName attributeName,
			Dictionary<ushort, IInventoryItemDetailConfig> details)
		{
			Id = id;
			Name = name;
			Rarity = rarity;
			_details = details;
			AttributeName = attributeName;
			SlotName = slotName;
		}
	}
}