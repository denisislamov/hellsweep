using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class BoostAllResponse
    {
        public List<Boost> items;

        [System.Serializable]
        public class Boost
        {
            public string id;
            public string name;
            public string description;
            public List<Detail> details;
        
            [System.Serializable]
            public class Detail
            {
                public int level;
                public int value;
            }
        }
    }
}