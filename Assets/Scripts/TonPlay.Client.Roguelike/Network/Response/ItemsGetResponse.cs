using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemsGetResponse 
    {
        public List<Item> items;
        
        [System.Serializable]
        public class Item
        {
            public string id;
            public string name;
            public string rarity;
            public string purpose;
            public string feature;
            public List<Detail> details;

            [System.Serializable]
            public class Detail
            {
                public string feature;
                public ushort level;
                public uint value;
            }
        }
    }
}