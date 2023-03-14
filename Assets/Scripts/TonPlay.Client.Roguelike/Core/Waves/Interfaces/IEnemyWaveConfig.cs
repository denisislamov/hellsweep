namespace TonPlay.Client.Roguelike.Core.Waves.Interfaces
{
	public interface IEnemyWaveConfig
	{
		string Id { get; }
		string EnemyId { get; }
		int EnemiesQuantity { get; }
		long StartTimingTicks { get; }
		int MaxSpawnedQuantity { get; }
		WaveSpawnType WaveSpawnType { get; }
	}
}