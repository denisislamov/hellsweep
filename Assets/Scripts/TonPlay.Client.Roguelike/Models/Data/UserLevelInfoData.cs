using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
    public class UserLevelInfoData : IData
    {
        public int Level { get; set; }

        public long Xp { get; set;}
		
        public long Coin { get; set;}
    }
}