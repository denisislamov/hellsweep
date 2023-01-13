namespace TonPlay.Roguelike.Client.Core.Waves.Interfaces
{
	public interface IWaveConfig
	{
		string Id { get; }
		string EnemyId { get; }
		int EnemiesQuantity { get; }
		int SpawnQuantityPerRate { get; }
		long SpawnTickRate { get; }
		int MaxSpawnedQuantity { get; }
	}
}