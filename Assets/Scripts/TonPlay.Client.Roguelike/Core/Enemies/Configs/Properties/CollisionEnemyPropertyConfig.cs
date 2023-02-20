using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(CollisionEnemyPropertyConfig), 
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(CollisionEnemyPropertyConfig))]
	public class CollisionEnemyPropertyConfig : EnemyPropertyConfig, ICollisionEnemyPropertyConfig
	{
		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;
		
		[SerializeField]
		private LayerMask _layerMask;
		
		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		public int LayerMask => _layerMask.value;
	}
}