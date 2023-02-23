using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DamageOnCollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DamageOnCollisionProjectileConfigProperty))]
	public class DamageOnCollisionProjectileConfigProperty : ProjectileConfigProperty, IDamageOnCollisionProjectileConfigProperty
	{
		[SerializeField]
		private DamageProvider _damageProvider;

		public IDamageProvider DamageProvider => _damageProvider;
	}
}