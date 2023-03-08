using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
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
        public int xp;
        public int coin;
    }
}