using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnMoveDistanceProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnMoveDistanceProjectileConfigProperty))]
	public class ExplodeOnMoveDistanceProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnMoveDistanceProjectileConfigProperty
	{
		[SerializeField]
		private float _distance;
		
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreConfig;
		
		[SerializeField]
		private DamageProvider _damage;

		public float Distance => _distance;
		public IDamageProvider DamageProvider => _damage;
		public ICollisionAreaConfig ExplodeCollisionAreaConfig => _explodeCollisionAreConfig;
	}
}