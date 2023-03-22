using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class LocationAllResponse
	{
		public List<Location> items;

		[System.Serializable]
		public class Location
		{
			public string id;
			public int chapter;
			public bool infinite;
		}
	}
}