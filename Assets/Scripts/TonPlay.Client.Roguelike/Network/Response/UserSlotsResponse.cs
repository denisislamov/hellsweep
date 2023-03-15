using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class UserSlotsResponse 
    {
        public List<Slot> _items;
        
        [System.Serializable]
        public class Slot
        {
            public string id;
            public string purpose;
            public ItemPutResponse.Item item;
        }
    }
}