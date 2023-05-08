using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class ShopPacksResponse 
	{
		public List<Pack> items;
        
		[System.Serializable]
		public class Pack
		{
			public string id;
			public uint priceTon;
			public uint energy;
			public ulong coins;
			public uint blueprints;
			public uint keysCommon;
			public uint keysUncommon;
			public uint keysRare;
			public uint keysLegendary;
			public uint heroSkins;
		}
	}
}