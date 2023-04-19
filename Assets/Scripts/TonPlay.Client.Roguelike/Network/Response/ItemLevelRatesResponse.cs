using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class ItemLevelRatesResponse 
	{
		public List<Item> items;
        
		[System.Serializable]
		public class Item
		{
			public string id;
			public long coins;
			public ushort blueprints;
		}
	}
}