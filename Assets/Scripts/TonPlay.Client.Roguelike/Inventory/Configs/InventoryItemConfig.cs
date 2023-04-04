using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemConfig : IInventoryItemConfig
	{
		public string Id { get; }
		public string Name { get; }
		public RarityName Rarity { get; }
		
		public InventoryItemConfig(string id, string name, RarityName rarity)
		{
			Id = id;
			Name = name;
			Rarity = rarity;
		}
	}
}