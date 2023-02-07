using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DestroyOnCollisionProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DestroyOnCollisionProjectileConfigProperty))]
	public class DestroyOnCollisionProjectileConfigProperty : ProjectileConfigProperty, IDestroyOnCollisionProjectileConfigProperty
	{
	}
}