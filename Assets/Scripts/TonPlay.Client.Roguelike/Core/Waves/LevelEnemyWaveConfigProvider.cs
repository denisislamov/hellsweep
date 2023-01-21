using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Waves
{
	[CreateAssetMenu(fileName = nameof(LevelEnemyWaveConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(LevelEnemyWaveConfigProvider))]
	public class LevelEnemyWaveConfigProvider : ScriptableObject, ILevelEnemyWaveConfigProvider
	{
		[SerializeField]
		private List<EnemyWaveGroupConfig> _waveConfigs;

		private List<EnemyWaveGroupConfig> _sortedWaveConfigs;

		private readonly List<IEnemyWaveConfig> _emptyList;

		public IEnumerable<IEnemyWaveConfig> AllWaves => _waveConfigs.SelectMany(_ => _.Waves);

		public IEnumerable<IEnemyWaveConfig> Get(long ticks)
		{
			if (_sortedWaveConfigs == null || _sortedWaveConfigs.Count != _waveConfigs.Count)
			{
				_sortedWaveConfigs = _waveConfigs.ToList();
				Sort();
			}

			EnemyWaveGroupConfig next = null;
			EnemyWaveGroupConfig current = null;
			
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

				if (next == default(EnemyWaveGroupConfig) || next.StartTimingTicks > ticks)
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
		private class EnemyWaveGroupConfig
		{
			[SerializeField]
			private List<EnemyWaveConfig> _waves;
			
			[SerializeField]
			private TimingConfig _startTiming;

			public long StartTimingTicks => _startTiming.GetTimeSpan().Ticks;

			public IEnumerable<EnemyWaveConfig> Waves => _waves;
		}
	}
}