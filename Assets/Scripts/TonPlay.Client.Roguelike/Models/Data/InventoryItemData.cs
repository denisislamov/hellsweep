using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class InventoryItemData : IData
	{
		public string Id { get; set; }
		public string ItemId { get; set; }
		public string DetailId { get; set; }
		public int LastUpdateIndex { get; set; }
	}
}