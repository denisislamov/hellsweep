using System;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Waves
{
	[Serializable]
	public class WaveConfig : IWaveConfig
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
		private int _spawnQuantityPerRate;

		[SerializeField]
		private TimingConfig _spawnRate;

		public string Id => _id;
		public string EnemyId => _enemyId;
		public int EnemiesQuantity => _enemiesQuantity;
		public int SpawnQuantityPerRate => _spawnQuantityPerRate;
		public long SpawnTickRate => _spawnRate.GetTimeSpan().Ticks;
		public int MaxSpawnedQuantity => _maxSpawnedQuantity;
	}
}