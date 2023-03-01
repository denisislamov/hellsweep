using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(fileName = nameof(CanSpawnProjectileEnemyPropertyConfig), menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(CanSpawnProjectileEnemyPropertyConfig))]
	public class CanSpawnProjectileEnemyPropertyConfig : EnemyPropertyConfig, ICanSpawnProjectileEnemyPropertyConfig
	{
		[SerializeField, Layer]
		private int _layer;
		
		[SerializeField]
		private int _pooledCount;

		[SerializeField]
		private ProjectileConfig _projectileConfig;

		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public int Layer => _layer;
		
		public int PooledCount => _pooledCount;
	}
}