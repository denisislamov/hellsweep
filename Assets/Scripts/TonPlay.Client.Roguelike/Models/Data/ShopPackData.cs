using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ShopPackData : IData
	{
		public string Id { get; set; }

		public float Price { get; set; }

		public ShopPackRewardsData Rewards { get; set; }
	}
}