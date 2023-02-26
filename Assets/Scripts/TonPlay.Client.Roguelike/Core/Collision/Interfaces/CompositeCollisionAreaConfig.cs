using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	[CreateAssetMenu(fileName = nameof(CompositeCollisionAreaConfig), menuName = AssetMenuConstants.COLLISION_AREAS_CONFIGS + nameof(CompositeCollisionAreaConfig))]
	internal class CompositeCollisionAreaConfig : CollisionAreaConfig, ICompositeCollisionAreaConfig
	{
		[SerializeField]
		private List<CollisionAreaConfig> _collisionAreaConfigs;

		public IReadOnlyList<ICollisionAreaConfig> CollisionAreaConfigs => _collisionAreaConfigs;
	}
}