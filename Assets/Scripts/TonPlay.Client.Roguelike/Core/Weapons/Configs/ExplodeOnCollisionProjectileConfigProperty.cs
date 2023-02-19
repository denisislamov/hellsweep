using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnCollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnCollisionProjectileConfigProperty))]
	public class ExplodeOnCollisionProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnCollisionProjectileConfigProperty
	{
		[SerializeField]
		private DamageProvider _damageProvider;
		
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreaConfig;

		public IDamageProvider DamageProvider => _damageProvider;
		public ICollisionAreaConfig ExplodeCollisionAreaConfig => _explodeCollisionAreaConfig;
	}
}