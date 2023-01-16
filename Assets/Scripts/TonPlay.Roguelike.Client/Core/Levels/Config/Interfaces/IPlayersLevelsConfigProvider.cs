namespace TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces
{
	public interface IPlayersLevelsConfigProvider
	{
		IPlayerLevelConfig Get(int level);
	}
}