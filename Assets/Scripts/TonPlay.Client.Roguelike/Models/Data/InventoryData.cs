using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class InventoryData : IData
	{
		public List<InventoryItemData> Items { get; set; }
		public Dictionary<SlotName, SlotData> Slots { get; set; }
	}
}