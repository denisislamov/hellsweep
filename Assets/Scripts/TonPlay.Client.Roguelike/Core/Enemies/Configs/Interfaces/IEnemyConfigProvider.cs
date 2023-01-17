namespace TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfigProvider
	{
		IEnemyConfig Get(string id = default);
	}
}