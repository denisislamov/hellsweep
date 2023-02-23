using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class BalanceData : IData
	{
		public int Gold { get; set; }

		public int Energy { get; set; }

		public int MaxEnergy { get; set; }
	}
}