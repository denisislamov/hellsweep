using System.Collections.Generic;

namespace TonPlay.Roguelike.Client.Core.Waves.Interfaces
{
	public interface ILevelWaveConfigProvider
	{
		IEnumerable<IWaveConfig> AllWaves { get; }
		
		IEnumerable<IWaveConfig> Get(long ticks);
	}
}