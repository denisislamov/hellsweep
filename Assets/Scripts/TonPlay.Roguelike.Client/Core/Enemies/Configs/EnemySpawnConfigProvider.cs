using System;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using IPlayerSpawnConfigProvider = TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces.IPlayerSpawnConfigProvider;

namespace TonPlay.Roguelike.Client.Core.Enemies.Configs
{
	[CreateAssetMenu(fileName = nameof(EnemySpawnConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(EnemySpawnConfigProvider))]
	public class EnemySpawnConfigProvider : ScriptableObject, IEnemySpawnConfigProvider
	{
		[SerializeField]
		private EnemySpawnConfig[] _configs;
		
		[SerializeField]
		private string _defaultConfigId;
		
		public IEnemySpawnConfig Get(string id = default)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = _defaultConfigId;
			}
			
			return _configs.FirstOrDefault(config => config.Id == id);
		}
		
		[Serializable]
		private class EnemySpawnConfig : IEnemySpawnConfig
		{
			[SerializeField]
			private string _id;
		
			[SerializeField]
			private EnemyView _prefab;

			[SerializeField]
			private int _health;
			
			[SerializeField]
			private int _damageOnCollision;

			public string Id => _id;
		
			public EnemyView Prefab => _prefab;
			
			public int StartHealth => _health;
			
			public int DamageOnCollision => _damageOnCollision;
		}
	}
}