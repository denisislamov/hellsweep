using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public class UserLevelsInfoData : IData
    {
        public List<UserLevelInfoData> Levels { get; set; } 
    }
}