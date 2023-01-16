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
		private readonly KdTreeStorage[] _kdTreeStorages;

		private List<int> _results = new List<int>(16);

		public OverlapExecutor(EcsWorld world, KdTreeStorage[] kdTreeStorages)
		{
			_world = world;
			_kdTreeStorages = kdTreeStorages;
		}

		public int Overlap(KDQuery query, Vector2 position, ICollisionAreaConfig collisionAreaConfig, ref List<int> entities, int layerMask)
		{
			var inactivePool = _world.GetPool<InactiveComponent>();
			var deadPool = _world.GetPool<DeadComponent>();
			var usedPool = _world.GetPool<UsedComponent>();
			var layerPool = _world.GetPool<LayerComponent>();

			for (var i = 0; i < _kdTreeStorages.Length; i++)
			{
				var kdTreeStorage = _kdTreeStorages[i];

				if (collisionAreaConfig is ICircleCollisionAreaConfig circleCollisionAreaConfig)
				{
					query.Radius(kdTreeStorage.KdTree, position, circleCollisionAreaConfig.Radius, _results);
				}
				
				for (var j = 0; j < _results.Count; j++)
				{
					var index = _results[j];
					var entityId = kdTreeStorage.KdTreePositionIndexToEntityIdMap[index];
					
					if (entityId == EcsEntity.DEFAULT_ID)
					{
						continue;
					}

					if (!layerPool.Has(entityId) ||
						inactivePool.Has(entityId) ||
						deadPool.Has(entityId) ||
						usedPool.Has(entityId))
					{
						continue;
					} 


					ref var layer = ref layerPool.Get(entityId);

					if (DoesLayerMaskContainsLayer(layerMask, layer))
					{
						entities.Add(entityId);
					}
				}
				
				_results.Clear();
			}

			return entities.Count;
		}
		
		private static bool DoesLayerMaskContainsLayer(int layerMask, LayerComponent layer)
		{
			return (layerMask & (1 << layer.Layer)) != 0;
		}

		public class Factory : PlaceholderFactory<EcsWorld, KdTreeStorage[], OverlapExecutor>
		{
		}
	}
}