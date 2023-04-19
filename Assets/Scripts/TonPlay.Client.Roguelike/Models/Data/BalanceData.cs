using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class BalanceData : IData
	{
		public long Gold { get; set; }

		public long Energy { get; set; }

		public long MaxEnergy { get; set; }
	}
}