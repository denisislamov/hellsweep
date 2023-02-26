using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(RicochetOffTheArenaProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(RicochetOffTheArenaProjectileConfigProperty))]
	public class RicochetOffTheArenaProjectileConfigProperty : ProjectileConfigProperty, IRicochetOffTheArenaProjectileConfigProperty
	{
		[SerializeField]
		private LayerMask _collisionLayerMask;
		
		public int CollisionLayerMask => _collisionLayerMask.value;
	}
}