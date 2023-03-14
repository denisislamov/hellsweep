using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
{
    public class UserSlotsResponse 
    {
        public List<Slot> _items;
        
        public class Slot
        {
            public string id;
            public string purpose;
            public ItemPutResponse.Item item;
        }
    }
}