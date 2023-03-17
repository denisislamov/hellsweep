using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class InfoLevelAllResponse
    {
        public List<InfoLevelAllItemResponse> items;
    }

    [System.Serializable]
    public class InfoLevelAllItemResponse
    {
        public int level;
        public long xp;
        public long coin;
    }
}