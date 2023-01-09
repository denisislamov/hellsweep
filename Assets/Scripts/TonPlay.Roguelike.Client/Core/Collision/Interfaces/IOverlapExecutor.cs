using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collision.Interfaces
{
	public interface IOverlapExecutor
	{
		public int Overlap(Vector2 position, ICollisionAreaConfig collisionAreaConfig, ref List<int> entitiesIds, int layerMask);
	}
}