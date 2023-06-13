using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ExplodeProjectileConfig), menuName = AssetMenuConstants.PROJECTILE_PROPERTIES_CONFIGS + nameof(ExplodeProjectileConfig))]
	public class ExplodeProjectileConfig : ScriptableObject, IExplodeProjectileConfig
	{
		[SerializeField]
		private CollisionAreaConfig _explodeCollisionAreConfig;
		
		[SerializeField] 
		private DamageProvider _damageProvider;
		
		[SerializeField] 
		private LayerMask _explodeCollisionLayerMask;

		public ICollisionAreaConfig ExplodeCollisionAreaConfig => _explodeCollisionAreConfig;
		public IDamageProvider DamageProvider => _damageProvider;
		public int ExplodeCollisionLayerMask => _explodeCollisionLayerMask;
	}
}