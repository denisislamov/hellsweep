using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
{
    [System.Serializable]
    public class ItemsGetResponse 
    {
        public List<ItemPutResponse.Item> _items;
    }
}