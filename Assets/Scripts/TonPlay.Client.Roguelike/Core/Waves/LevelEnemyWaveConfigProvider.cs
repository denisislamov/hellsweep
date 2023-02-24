using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
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

		public IEnemyWaveGroupConfig Get(long ticks)
		{
			if (_sortedWaveConfigs == null || _sortedWaveConfigs.Count != _waveConfigs.Count)
			{
				_sortedWaveConfigs = _waveConfigs.ToList();
				Sort();
				SetNextForEachGroup();
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
					return null;
				}

				if (next == default(EnemyWaveGroupConfig) || next.StartTimingTicks > ticks)
				{
					return current;
				}
			}

			return null;
		}

		private void SetNextForEachGroup()
		{
			for (var i = 0; i < _sortedWaveConfigs.Count; i++)
			{
				var group = _sortedWaveConfigs[i];

				if (i + 1 < _sortedWaveConfigs.Count)
				{
					group.SetNext(_sortedWaveConfigs[i + 1]);
				}
			}
		}

		private void Sort()
		{
			_sortedWaveConfigs.Sort((a, b) => a.StartTimingTicks < b.StartTimingTicks ? -1 : 1);
		}

		[Serializable]
		private class EnemyWaveGroupConfig : IEnemyWaveGroupConfig
		{
			[SerializeField]
			private List<EnemyWaveConfig> _waves;

			[SerializeField]
			private TimingConfig _startTiming;

			private IEnemyWaveGroupConfig _next;

			public long StartTimingTicks => _startTiming.GetTimeSpan().Ticks;

			public IReadOnlyList<EnemyWaveConfig> Waves => _waves;

			public IEnemyWaveGroupConfig Next() => _next;

			public void SetNext(IEnemyWaveGroupConfig next) => _next = next;
		}
	}
}