using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ShopResourceData : IData
	{
		public ShopResourceType Type { get; set; }
		public RarityName Rarity { get; set; }
		public ulong Amount { get; set; }
		public float Price { get; set; }
		public string Id { get; set; }
	}
}