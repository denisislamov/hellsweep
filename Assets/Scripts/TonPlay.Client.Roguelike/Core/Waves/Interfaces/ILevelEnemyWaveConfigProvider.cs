using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Waves.Interfaces
{
	public interface ILevelEnemyWaveConfigProvider
	{
		IEnumerable<IEnemyWaveConfig> AllWaves { get; }
		
		IEnemyWaveGroupConfig Last { get; }

		IEnemyWaveGroupConfig Get(long ticks);
	}
}