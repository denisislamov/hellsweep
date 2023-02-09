namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfigProvider
	{
		IEnemyConfig Get(string id = default);
	}
}