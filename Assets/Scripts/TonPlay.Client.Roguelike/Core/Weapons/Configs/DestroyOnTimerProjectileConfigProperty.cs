using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(DestroyOnTimerProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(DestroyOnTimerProjectileConfigProperty))]
	public class DestroyOnTimerProjectileConfigProperty : ProjectileConfigProperty, IDestroyOnTimerProjectileConfigProperty
	{
		[SerializeField]
		private float _timer;
		
		public float Timer => _timer;
	}
}