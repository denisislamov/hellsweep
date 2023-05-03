using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemMergeResponse
    {
        public string id;
        public int level;
        public int value;
        public Item item;
        
        [System.Serializable]
        public class Item
        {
            public string id;
            public string name;
            public string rarity;
            public string purpose;
            public string feature;
            public List<string> details;
        }
    }
}