namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public class UserLevelInfoData : IData
    {
        public int Level { get; set; }

        public int Xp { get; set;}
		
        public int Coin { get; set;}
    }
}