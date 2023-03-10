using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
{
    [System.Serializable]
    public class GameSessionPutBody
    {
        public List<string> lootedItems;
        public int surviveMills;
    }
}