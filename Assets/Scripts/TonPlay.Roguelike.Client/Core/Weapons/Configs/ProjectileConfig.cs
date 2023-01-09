using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ProjectileConfig), menuName = AssetMenuConstants.PROJECTILE_CONFIGS + nameof(ProjectileConfig))]
	internal class ProjectileConfig : ScriptableObject, IProjectileConfig
	{
		[SerializeField]
		private ProjectileView _prefab;
		
		[SerializeField]
		private float _startSpeed;
		
		[SerializeField]
		private float _acceleration;
		
		[SerializeField]
		private int _damage;
		
		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;

		public ProjectileView PrefabView => _prefab;
		public float StartSpeed => _startSpeed;
		public float Acceleration => _acceleration;
		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		public int Damage => _damage;
	}
}