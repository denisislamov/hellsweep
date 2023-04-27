using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class ItemRewardData : IData
	{
		public string Id { get; set; }
		public ushort Count { get; set; }
	}
}