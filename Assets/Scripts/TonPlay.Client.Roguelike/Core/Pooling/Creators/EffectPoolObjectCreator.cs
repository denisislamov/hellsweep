using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Creators
{
	[CreateAssetMenu(fileName = nameof(EffectPoolObjectCreator), menuName = AssetMenuConstants.POOLING_CONFIGS + nameof(EffectPoolObjectCreator))]
	public class EffectPoolObjectCreator : PoolObjectCreator<EffectView>
	{
		[SerializeField]
		private EffectView _effectView;

		protected override EffectView Prefab => _effectView;
		
		protected override IViewPoolIdentity Identity => new GameObjectViewPoolIdentity(_effectView.gameObject);
	}
}