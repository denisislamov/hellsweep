namespace TonPlay.Client.Common.Network
{
    [System.Serializable]
    public class ItemPutResponse 
    {
        public string id;
        public string purpose;
        public Item item;

        [System.Serializable]
        public class Item
        {
            public string id;
            public string name;
            public string rarity;
        }
    }
}