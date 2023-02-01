using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DestroyIfRadiusExceededProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DestroyIfRadiusExceededProjectileConfigProperty))]
	public class DestroyIfRadiusExceededProjectileConfigProperty : ProjectileConfigProperty, IDestroyIfRadiusExceededProjectileConfigProperty
	{
		[SerializeField]
		private float _distance;
		
		public float Distance => _distance;
	}
}