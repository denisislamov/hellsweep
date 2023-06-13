using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ExplodeOnMoveDistanceComponent
	{
		public Vector2 StartPosition;
		public IDamageProvider DamageProvider;
		public float DistanceToExplode;
		public ICollisionAreaConfig CollisionConfig;
		public int CollisionLayerMask;
	}
}