using System.Collections.Generic;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class UserSlotsResponse 
    {
        public List<Slot> items;
        
        [System.Serializable]
        public class Slot
        {
            public string id;
            public string userItemId;
            public string purpose;
        }
    }
}