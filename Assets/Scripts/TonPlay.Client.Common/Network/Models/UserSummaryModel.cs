namespace TonPlay.Client.Common.Network
{
    [System.Serializable]
    public class Profile
    {
        public string id;
        public string firstname;
        public string lastname;
        public string avatar;
        public string nickname;
        public string telegram;
        public int level;
        public int xp;
        public int coin;
        public int energy;
    }

    [System.Serializable]
    public class UserSummaryModel
    {
        public string id;
        public string identifier;
        public string username;
        public string status;
        public Profile profile;
    }
}