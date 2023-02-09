using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	[CreateAssetMenu(fileName = nameof(AABBCollisionAreaConfig), menuName = AssetMenuConstants.COLLISION_AREAS_CONFIGS + nameof(AABBCollisionAreaConfig))]
	internal class AABBCollisionAreaConfig : CollisionAreaConfig, IAABBCollisionAreaConfig
	{
		[SerializeField]
		private Rect _rect;

		public Rect Rect => _rect;
	}
}