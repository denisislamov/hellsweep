using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeOnTimerProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeOnTimerProjectileConfigProperty))]
	public class ExplodeOnTimerProjectileConfigProperty : ProjectileConfigProperty, IExplodeOnTimerProjectileConfigProperty
	{
		[SerializeField]
		private float _timer;

		[SerializeField] 
		private ExplodeProjectileConfig _explodeProjectileConfig;

		public float Timer => _timer;
		
		public IExplodeProjectileConfig ExplodeProjectileConfig => _explodeProjectileConfig;
	}
}