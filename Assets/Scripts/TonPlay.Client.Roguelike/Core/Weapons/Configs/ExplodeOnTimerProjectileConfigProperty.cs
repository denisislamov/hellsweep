using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnTimerProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnTimerProjectileConfigProperty))]
	public class ExplodeOnTimerProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnTimerProjectileConfigProperty
	{
		[SerializeField]
		private float _timer;
		
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreConfig;

		public float Timer => _timer;
		public ICollisionAreaConfig ExplodeCollisionAreConfig => _explodeCollisionAreConfig;
	}
}