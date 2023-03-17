using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public class UserLevelsInfoData : IData
    {
        public List<UserLevelInfoData> Levels { get; set; } 
    }
}