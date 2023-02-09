using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Drops;
using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs
{
	[CreateAssetMenu(fileName = nameof(EnemyConfig), menuName = AssetMenuConstants.ENEMIES_CONFIGS + nameof(EnemyConfig))]
	internal class EnemyConfig : ScriptableObject, IEnemyConfig
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private EnemyView _prefab;

		[SerializeField]
		private int _health;

		[SerializeField]
		private DamageProvider _damageProviderOnCollision;

		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;

		[SerializeField]
		private MovementConfig _movementConfig;

		[SerializeField]
		private WeightedCollectableIdDropConfig[] _collectablesIdsOnDeath;
		
		private IItemDrop<string>[] _randomCollectablesDrop;

		public string Id => _id;

		public EnemyView Prefab => _prefab;

		public int StartHealth => _health;

		public IDamageProvider DamageProvider => _damageProviderOnCollision;

		public IMovementConfig MovementConfig => _movementConfig;

		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		public IItemDrop<string>[] RandomCollectableDrops => _randomCollectablesDrop ??= _collectablesIdsOnDeath.Select(_ => new RandomCollectableIdDrop(_)).ToArray();
	}
}