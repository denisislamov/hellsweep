namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class UserItemResponse 
    {
        public string id;
        public ushort level;
        public Item item;

        [System.Serializable]
        public class Item
        {
            public string id;
        }
    }
}