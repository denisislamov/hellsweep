using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Core.Collision
{
	public class OverlapExecutor : IOverlapExecutor
	{
		private readonly EcsWorld _world;
		private readonly KdTreeStorage _kdTreeStorage;

		private List<int> _results = new List<int>(16);
		
		public OverlapExecutor(EcsWorld world, KdTreeStorage kdTreeStorage)
		{
			_world = world;
			_kdTreeStorage = kdTreeStorage;
		}
		
		public int Overlap(Vector2 position, ICollisionAreaConfig collisionAreaConfig, ref List<int> entities, int layerMask)
		{
			var inactivePool = _world.GetPool<InactiveComponent>();
			var deadPool = _world.GetPool<DeadComponent>();
			
			if (collisionAreaConfig is ICircleCollisionAreaConfig circleCollisionAreaConfig)
			{
				_kdTreeStorage.KdQuery.Radius(_kdTreeStorage.KdTree, position, circleCollisionAreaConfig.Radius, _results);

				for (var i = 0; i < _results.Count; i++)
				{
					var index = _results[i];
					var entityId = _kdTreeStorage.KdTreePositionIndexToEntityIdMap[index];

					if (inactivePool.Has(entityId) || deadPool.Has(entityId))
					{
						continue;
					}
					
					entities.Add(entityId);
				}
				
				_results.Clear();

				return entities.Count;
			}
			
			return 0;
		}
		
		public class Factory : PlaceholderFactory<EcsWorld, KdTreeStorage, OverlapExecutor>
		{
		}
	}
}