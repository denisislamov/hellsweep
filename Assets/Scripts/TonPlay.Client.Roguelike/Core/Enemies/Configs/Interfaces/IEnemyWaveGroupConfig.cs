using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Waves;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyWaveGroupConfig
	{
		long StartTimingTicks { get; }

		IReadOnlyList<EnemyWaveConfig> Waves { get; }

		IEnemyWaveGroupConfig Next();
	}
}