namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class GameSessionResponse 
    {
        public string id;
        public string status;
        public bool pve;
    }
}