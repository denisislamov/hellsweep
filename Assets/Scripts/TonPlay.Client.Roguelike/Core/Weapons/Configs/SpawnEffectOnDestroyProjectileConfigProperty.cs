using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(SpawnEffectOnDestroyProjectileConfigProperty), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(SpawnEffectOnDestroyProjectileConfigProperty))]
	public class SpawnEffectOnDestroyProjectileConfigProperty : ProjectileConfigProperty, ISpawnEffectOnDestroyProjectileConfigProperty
	{
		[SerializeField] 
		private EffectView _effectView;
		
		[SerializeField] 
		private float _destroyTimer;
		
		private GameObjectViewPoolIdentity _identity;

		public IEffectView EffectView => _effectView;
		
		public IViewPoolIdentity EffectViewPoolIdentity => _identity ??= new GameObjectViewPoolIdentity(_effectView.gameObject);
		
		public float DestroyTimer => _destroyTimer;
	}
}