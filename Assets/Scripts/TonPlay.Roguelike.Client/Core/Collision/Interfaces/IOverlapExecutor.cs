using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collision.Interfaces
{
	public interface IOverlapExecutor
	{
		public int Overlap(KDQuery query, Vector2 position, ICollisionAreaConfig collisionAreaConfig, ref List<int> entitiesIds, int layerMask);
	}
}