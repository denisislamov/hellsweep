namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class CloseGameSessionPostBody
    {
        public long coins;
        public long surviveMills;
        public long killed;
    }
}