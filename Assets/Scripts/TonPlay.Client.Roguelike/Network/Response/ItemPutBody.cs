namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class ItemPutBody 
    {
        public string slotId;
        public string userItemId;
    }
    [System.Serializable]
    public class ItemLevelUpPutBody 
    {
        public string id;
        public bool isMax;
    }
}