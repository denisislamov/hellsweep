using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class CloseGameSessionPostBody
    {
        public long coins;
        public long surviveMills;
    }
}