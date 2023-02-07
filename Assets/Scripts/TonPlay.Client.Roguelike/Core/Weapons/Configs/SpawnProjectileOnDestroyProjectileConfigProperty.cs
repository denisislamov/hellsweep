using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(SpawnProjectileOnDestroyProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(SpawnProjectileOnDestroyProjectileConfigProperty))]
	public class SpawnProjectileOnDestroyProjectileConfigProperty : ProjectileConfigProperty, ISpawnProjectileOnDestroyProjectileConfigProperty
	{
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private LayerMask _collisionLayerMask;

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		
		public int CollisionLayerMask => _collisionLayerMask.value;
	}
}