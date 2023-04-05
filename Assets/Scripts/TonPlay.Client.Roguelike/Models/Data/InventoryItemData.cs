using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class InventoryItemData : IData
	{
		public string Id { get; set; }
		public string DetailId { get; set; }
		public ushort Level { get; set; }
		// public string Name { get; set; }
		// public RarityName Rarity { get; set; }
	}
}