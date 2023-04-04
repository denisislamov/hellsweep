using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ProfileData : IData
	{
		public BalanceData BalanceData { get; set; }
		
		public InventoryData InventoryData { get; set; }

		public float Experience { get; set; }

		public float MaxExperience { get; set; }

		public int Level { get; set; }
	}
}