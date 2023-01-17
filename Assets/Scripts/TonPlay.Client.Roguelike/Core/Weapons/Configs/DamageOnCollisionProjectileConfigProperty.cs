using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DamageOnCollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DamageOnCollisionProjectileConfigProperty))]
	public class DamageOnCollisionProjectileConfigProperty : ProjectileConfigProperty, IDamageOnCollisionProjectileConfigProperty
	{
		[SerializeField]
		private int _damage;
		
		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;
		
		public int Damage => _damage;
		
		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
	}
}