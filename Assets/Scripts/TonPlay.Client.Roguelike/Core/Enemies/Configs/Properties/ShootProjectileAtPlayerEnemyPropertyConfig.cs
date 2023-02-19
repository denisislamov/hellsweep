using System;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(fileName = nameof(ShootProjectileAtPlayerEnemyPropertyConfig), menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(ShootProjectileAtPlayerEnemyPropertyConfig))]
	public class ShootProjectileAtPlayerEnemyPropertyConfig : EnemyPropertyConfig, IShootProjectileAtPlayerEnemyPropertyConfig
	{
		[SerializeField]
		private float _shootRateInSeconds;

		[SerializeField, Layer]
		private int _layer;
		
		[SerializeField]
		private float _minDistanceToTargetToShoot;
		
		[SerializeField]
		private float _maxDistanceToTargetToShoot;

		[SerializeField]
		private ProjectileConfig _projectileConfig;

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		
		public float ShootRateInSeconds => _shootRateInSeconds;
		
		public int Layer => _layer;
		
		public float MinDistanceToTargetToShoot => _minDistanceToTargetToShoot;
		
		public float MaxDistanceToTargetToShoot => _maxDistanceToTargetToShoot;
	}
}