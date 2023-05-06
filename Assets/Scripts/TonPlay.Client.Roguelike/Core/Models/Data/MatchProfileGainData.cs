using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class MatchProfileGainData
	{
		public int Gold { get; set; }
		public float ProfileExperience { get; set; }
		public List<InventoryItemData> Items { get; set; }
	}
}