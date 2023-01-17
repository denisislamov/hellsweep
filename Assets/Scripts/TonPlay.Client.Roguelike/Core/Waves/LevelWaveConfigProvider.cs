using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Roguelike.Client.Core.Waves
{
	[CreateAssetMenu(fileName = nameof(LevelWaveConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(LevelWaveConfigProvider))]
	public class LevelWaveConfigProvider : ScriptableObject, ILevelWaveConfigProvider
	{
		[SerializeField]
		private List<WaveGroupConfig> _waveConfigs;

		private List<WaveGroupConfig> _sortedWaveConfigs;

		private readonly List<IWaveConfig> _emptyList;

		public IEnumerable<IWaveConfig> AllWaves => _waveConfigs.SelectMany(_ => _.Waves);

		public IEnumerable<IWaveConfig> Get(long ticks)
		{
			if (_sortedWaveConfigs == null || _sortedWaveConfigs.Count != _waveConfigs.Count)
			{
				_sortedWaveConfigs = _waveConfigs.ToList();
				Sort();
			}

			WaveGroupConfig next = null;
			WaveGroupConfig current = null;
			
			for (int i = 0; i < _sortedWaveConfigs.Count; i++)
			{
				current = _sortedWaveConfigs[i];

				next = i + 1 < _sortedWaveConfigs.Count 
					? _sortedWaveConfigs[i + 1] 
					: default;

				if (ticks < current.StartTimingTicks)
				{
					return _emptyList;
				}

				if (next == default(WaveGroupConfig) || next.StartTimingTicks > ticks)
				{
					return current.Waves;
				}
			}

			return _emptyList;
		}

		private void Sort()
		{
			_sortedWaveConfigs.Sort((a, b) => a.StartTimingTicks < b.StartTimingTicks ? -1 : 1);
		}

		[Serializable]
		private class WaveGroupConfig
		{
			[SerializeField]
			private List<WaveConfig> _waves;
			
			[SerializeField]
			private TimingConfig _startTiming;

			public long StartTimingTicks => _startTiming.GetTimeSpan().Ticks;

			public IEnumerable<WaveConfig> Waves => _waves;
		}
	}
}