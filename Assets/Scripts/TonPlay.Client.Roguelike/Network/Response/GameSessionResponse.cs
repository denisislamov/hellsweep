using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class GameSessionResponse 
    {
        public string id;
        public string status;
        public bool pve;

        public RewardSummary rewardSummary;
        
        [System.Serializable]
        public class Chest
        {
            public string id;
            public string rarity;
        }
        
        [System.Serializable]
        public class RewardSummary
        {
            public int xp;
            public int coin;
            public int energy;
            public List<Chest> chests;
        }
    }
}