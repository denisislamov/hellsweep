using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Effects.Death
{
	[CreateAssetMenu(fileName = nameof(DeathEffectConfig), menuName = AssetMenuConstants.CONFIGS + nameof(DeathEffectConfig))]
	public class DeathEffectConfig : ScriptableObject, IDeathEffectConfig
	{
		[SerializeField]
		private DeathEffectView _prefab;
		
		[SerializeField]
		private float _destroyTimer;
		
		private IViewPoolIdentity _identity;

		public IViewPoolIdentity Identity => _identity ?? new GameObjectViewPoolIdentity(_prefab.gameObject);
		
		public DeathEffectView Prefab => _prefab;
		
		public float DestroyTimer => _destroyTimer;
	}
}