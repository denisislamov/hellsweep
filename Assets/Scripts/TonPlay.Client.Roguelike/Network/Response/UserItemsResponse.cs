using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class UserItemsResponse
    {
        public List<UserItemResponse> items;
    }
}