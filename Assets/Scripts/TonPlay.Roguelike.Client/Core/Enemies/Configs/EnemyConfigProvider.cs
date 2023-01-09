using System;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Enemies.Configs
{
	[CreateAssetMenu(fileName = nameof(EnemyConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(EnemyConfigProvider))]
	public class EnemyConfigProvider : ScriptableObject, IEnemyConfigProvider
	{
		[SerializeField]
		private EnemyConfig[] _configs;
		
		[SerializeField]
		private string _defaultConfigId;
		
		public IEnemyConfig Get(string id = default)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = _defaultConfigId;
			}
			
			return _configs.FirstOrDefault(config => config.Id == id);
		}
		
		[Serializable]
		private class EnemyConfig : IEnemyConfig
		{
			[SerializeField]
			private string _id;
		
			[SerializeField]
			private EnemyView _prefab;

			[SerializeField]
			private int _health;
			
			[SerializeField]
			private int _damageOnCollision;
			
			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public string Id => _id;
		
			public EnemyView Prefab => _prefab;
			
			public int StartHealth => _health;
			
			public int DamageOnCollision => _damageOnCollision;
			
			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		}
	}
}