using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(BombCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(BombCollectableConfig))]
	public class BombCollectableConfig : CollectableConfig, IBombCollectableConfig
	{
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreaConfig;
		
		[SerializeField]
		private float _timeToExplode;
		
		[SerializeField]
		private float _damage;
		
		[SerializeField]
		private LayerMask _layerMask;

		public override CollectableType Type => CollectableType.Bomb;
		
		public ICollisionAreaConfig CollisionAreaConfig => _explodeCollisionAreaConfig;
		
		public float TimeToExplode => _timeToExplode;
		public float Damage => _damage;
		public int LayerMask => _layerMask.value;
	}
}