using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemsGetResponse 
    {
        public List<ItemPutResponse.Item> _items;
    }
}