using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models;
using UnityEngine.Serialization;

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
        public class RewardSummary
        {
            public int xp;
            public int coins;
            public int blueprints;
            public int energy;
            public List<UserItemResponse> items;
            public SlotName blueprintsSlotPurpose;
        }
    }
}