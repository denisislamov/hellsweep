using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnMoveDistanceProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnMoveDistanceProjectileConfigProperty))]
	public class ExplodeOnMoveDistanceProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnMoveDistanceProjectileConfigProperty
	{
		[SerializeField]
		private float _distance;
		
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreConfig;
		
		[SerializeField]
		private int _damage;

		public float Distance => _distance;
		public int Damage => _damage;
		public ICollisionAreaConfig ExplodeCollisionAreaConfig => _explodeCollisionAreConfig;
	}
}