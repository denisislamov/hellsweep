using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class UserLocationsResponse
	{
		public List<UserLocation> items;

		[System.Serializable]
		public class UserLocation
		{
			public string id;
			public int chapter;
			public bool infinite;
			public long surviveMills;
			public long maxKilled;
		}
	}
}