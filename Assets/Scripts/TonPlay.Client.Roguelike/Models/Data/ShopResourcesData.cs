using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ShopResourcesData : IData
	{
		public List<ShopResourceData> Items { get; set; } = new List<ShopResourceData>();
	}
}