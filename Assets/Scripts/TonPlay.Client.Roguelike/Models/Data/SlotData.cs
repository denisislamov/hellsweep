using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class SlotData : IData
	{
		public SlotName SlotName { get; set; }
		public string Id { get; set; }
		public string ItemId { get; set; }
	}
}