using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Skills;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class SkillAllResponse
    {
        public List<Skill> items;
        [System.Serializable]
        public class Skill
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
                public float value;
            }
        }
    }
    
    [System.Serializable]
    public class UnitAllResponse
    {
        public List<Unit> items;
        [System.Serializable]
        public class Unit
        {
            public string id;
            public string name;
            public int timing;
            public int speed;
            public int quantity;
            public int frequency;
            public int health;
            public int damage;
        }
    }
}