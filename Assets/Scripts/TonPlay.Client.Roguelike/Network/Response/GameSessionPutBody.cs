using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class GameSessionPutBody
    {
        public List<string> lootedItems;
        public int surviveMills;
    }
}