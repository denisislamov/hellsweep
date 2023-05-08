using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ShopData : IData
	{
		public List<ShopPackData> Packs { get; set; } = new List<ShopPackData>();
		public List<ShopResourceData> Resources { get; set; } = new List<ShopResourceData>();
	}
}