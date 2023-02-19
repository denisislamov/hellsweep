using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Collision
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

		public int Overlap(
			KDQuery query, 
			Vector2 position, 
			ICollisionAreaConfig collisionAreaConfig, 
			ref List<int> entities, 
			int layerMask,
			IOverlapPools pools)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var inactivePool = pools.InactivePool;
			var deadPool = pools.DeadPool;
			var usedPool = pools.UsedPool;
			var layerPool = pools.LayerPool;
			var destroyPool = pools.DestroyPool;

			for (var i = 0; i < _kdTreeStorages.Length; i++)
			{
				var kdTreeStorage = _kdTreeStorages[i];

				if (!DoesLayerMaskContainsLayer(layerMask, kdTreeStorage.Layer))
				{
					continue;
				}

				if (collisionAreaConfig is ICircleCollisionAreaConfig circleCollisionAreaConfig)
				{
					query.Radius(kdTreeStorage.KdTree, position, circleCollisionAreaConfig.Radius, _results);
					
					for (var j = 0; j < _results.Count; j++)
					{
						var index = _results[j];
						var entityId = kdTreeStorage.KdTreePositionIndexToEntityIdMap[index];

						if (entityId == EcsEntity.DEFAULT_ID || !_world.IsEntityAlive(entityId))
						{
							continue;
						}

						if (!layerPool.Has(entityId) ||
							destroyPool.Has(entityId) ||
							inactivePool.Has(entityId) ||
							deadPool.Has(entityId) ||
							usedPool.Has(entityId))
						{
							continue;
						}
						
						ref var layer = ref layerPool.Get(entityId);

						if (DoesLayerMaskContainsLayer(layerMask, layer.Layer))
						{
							entities.Add(entityId);
						}
					}
				}
				else if (collisionAreaConfig is IAABBCollisionAreaConfig aabbCollisionAreaConfig)
				{
					var sourceRect = aabbCollisionAreaConfig.Rect;
					var rect = new Rect(position - sourceRect.center, sourceRect.size);
					var radius = Mathf.Max(sourceRect.max.magnitude, sourceRect.min.magnitude);

					query.Radius(kdTreeStorage.KdTree, position, radius, _results);
					
					for (var j = 0; j < _results.Count; j++)
					{
						var index = _results[j];
						var entityId = kdTreeStorage.KdTreePositionIndexToEntityIdMap[index];

						if (entityId == EcsEntity.DEFAULT_ID || !_world.IsEntityAlive(entityId))
						{
							continue;
						}

						if (!layerPool.Has(entityId) ||
							destroyPool.Has(entityId) ||
							inactivePool.Has(entityId) ||
							deadPool.Has(entityId) ||
							usedPool.Has(entityId))
						{
							continue;
						}

						if (!rect.Contains(kdTreeStorage.KdTree.Points[index]))
						{
							continue;
						}

						ref var layer = ref layerPool.Get(entityId);

						if (DoesLayerMaskContainsLayer(layerMask, layer.Layer))
						{
							entities.Add(entityId);
						}
					}
				}

				_results.Clear();
			}
			
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion

			return entities.Count;
		}

		private static bool DoesLayerMaskContainsLayer(int layerMask, int layer)
		{
			return (layerMask & (1 << layer)) != 0;
		}

		public class Factory : PlaceholderFactory<EcsWorld, KdTreeStorage[], OverlapExecutor>
		{
		}
	}
}