using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class ShopPackRewardsData : IData
	{
		public uint Energy { get; set; }
		
		public ulong Coins { get; set; }
		
		public uint Blueprints { get; set; }
		
		public uint KeysCommon { get; set; }
		
		public uint KeysUncommon { get; set; }
		
		public uint KeysRare { get; set; }
		
		public uint KeysLegendary { get; set; }
		
		public uint HeroSkins { get; set; }
	}
}