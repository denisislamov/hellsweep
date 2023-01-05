namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerSpawnConfigProvider
	{
		IPlayerSpawnConfig Get(string id = default);
	}
}