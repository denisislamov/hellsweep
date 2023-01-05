namespace TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces
{
	public interface IEnemySpawnConfigProvider
	{
		IEnemySpawnConfig Get(string id = default);
	}
}