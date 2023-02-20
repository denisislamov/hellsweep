using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct WavesDataComponent
	{
		public Dictionary<string, int> WavesEnemiesKilledAmount;
		public Dictionary<string, int> WavesEnemiesSpawnedAmount;
		public Dictionary<string, long> WavesTimeLeftToNextSpawn;
	}
}