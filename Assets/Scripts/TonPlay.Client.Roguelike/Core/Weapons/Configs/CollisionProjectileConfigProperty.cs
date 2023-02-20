using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(CollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(CollisionProjectileConfigProperty))]
	public class CollisionProjectileConfigProperty : ProjectileConfigProperty, ICollisionProjectileConfigProperty
	{
		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;

		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
	}
}