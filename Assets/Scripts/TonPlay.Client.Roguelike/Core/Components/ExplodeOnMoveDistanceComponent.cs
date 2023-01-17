using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct ExplodeOnMoveDistanceComponent
	{
		public Vector2 StartPosition;
		public int Damage;
		public float DistanceToExplode;
		public ICollisionAreaConfig CollisionConfig;
	}
}