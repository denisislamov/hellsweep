using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
	[System.Serializable]
	public class ItemLootResponse
	{
		public List<UserItemResponse> items;
	}
}