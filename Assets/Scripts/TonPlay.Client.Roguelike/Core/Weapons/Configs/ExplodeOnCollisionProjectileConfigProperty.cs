using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnCollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnCollisionProjectileConfigProperty))]
	public class ExplodeOnCollisionProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnCollisionProjectileConfigProperty
	{
		[SerializeField] 
		private ExplodeProjectileConfig _explodeProjectileConfig;

		public IExplodeProjectileConfig ExplodeProjectileConfig => _explodeProjectileConfig;
	}
}