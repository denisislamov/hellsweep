using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class UserInfoLevelAllResponse
    {
        public List<UserInfoLevelAllItemResponse> items;
    }

    [System.Serializable]
    public class UserInfoLevelAllItemResponse
    {
        public int level;
        public int xp;
        public int coin;
    }
}