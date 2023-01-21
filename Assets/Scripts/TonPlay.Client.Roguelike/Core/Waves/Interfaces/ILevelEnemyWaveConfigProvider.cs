using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Waves.Interfaces
{
	public interface ILevelEnemyWaveConfigProvider
	{
		IEnumerable<IEnemyWaveConfig> AllWaves { get; }
		
		IEnumerable<IEnemyWaveConfig> Get(long ticks);
	}
}