namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class ShopResourcesResponse 
	{
		public Resource blueprints;
		public Resource energy;
		public Resource coins;
		public GradeResource keys;
		public GradeResource items;
        
		[System.Serializable]
		public class Resource
		{
			public ulong amount;
			public float price;
		}
		
		[System.Serializable]
		public class GradeResource
		{
			public Resource COMMON;
			public Resource UNCOMMON;
			public Resource RARE;
			public Resource LEGENDARY;
		}
	}
}