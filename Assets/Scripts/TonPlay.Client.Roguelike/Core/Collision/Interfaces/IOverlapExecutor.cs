using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	public interface IOverlapExecutor
	{
		public int Overlap(
			KDQuery query,
			Vector2 position,
			ICollisionAreaConfig collisionAreaConfig,
			ref List<int> entities,
			int layerMask,
			IOverlapParams overlapParams);
	}
}