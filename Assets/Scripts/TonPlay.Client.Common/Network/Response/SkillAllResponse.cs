using System.Collections.Generic;

namespace TonPlay.Client.Common.Network
{
    [System.Serializable]
    public class SkillAllResponse 
    {
         public string id;
        public string name;
        public string rarity;
        public string description;
        public List<Detail> details;
    
        [System.Serializable]
        public class Detail
        {
            public string feature;
            public int level;
            public int value;
        }
    }
}