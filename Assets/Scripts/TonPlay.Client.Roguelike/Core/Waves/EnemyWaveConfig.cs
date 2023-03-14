using System;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Waves
{
	[Serializable]
	public class EnemyWaveConfig : IEnemyWaveConfig
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private string _enemyId;

		[SerializeField]
		private int _enemiesQuantity;

		[SerializeField]
		private int _maxSpawnedQuantity;

		[SerializeField]
		private TimingConfig _spawnRate;

		[SerializeField]
		private WaveSpawnType _waveSpawnType;
		
		[SerializeField]
		private TimingConfig _startTiming;
		
		[SerializeField]
		public float StartHealth;

		public string Id => _id;
		public string EnemyId => _enemyId;
		public int EnemiesQuantity => _enemiesQuantity;
		public long SpawnTickRate => _spawnRate.GetTimeSpan().Ticks;
		public long StartTimingTicks => _startTiming.GetTimeSpan().Ticks;
		public int MaxSpawnedQuantity => _maxSpawnedQuantity;
		public WaveSpawnType WaveSpawnType => _waveSpawnType;
	}
}