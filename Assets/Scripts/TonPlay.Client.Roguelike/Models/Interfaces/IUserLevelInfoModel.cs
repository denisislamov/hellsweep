namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public interface IUserLevelInfoModel : IModel<UserLevelInfoData>
    {
        public int Level { get; }

        public int Xp { get; }
		
        public int Coin { get; }
    }
}