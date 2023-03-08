using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
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