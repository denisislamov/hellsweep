using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DestroyOnReceiveDamageProjectileConfigProperty), 
		menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DestroyOnReceiveDamageProjectileConfigProperty))]
	public class DestroyOnReceiveDamageProjectileConfigProperty : ProjectileConfigProperty, IDestroyOnReceiveDamageProjectileConfigProperty
	{
	}
}