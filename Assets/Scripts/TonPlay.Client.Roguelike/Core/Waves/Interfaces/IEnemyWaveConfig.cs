namespace TonPlay.Client.Roguelike.Core.Waves.Interfaces
{
	public interface IEnemyWaveConfig
	{
		string Id { get; }
		string EnemyId { get; }
		int EnemiesQuantity { get; }
		int SpawnQuantityPerRate { get; }
		long SpawnTickRate { get; }
		int MaxSpawnedQuantity { get; }
		WaveSpawnType WaveSpawnType { get; }
	}
}