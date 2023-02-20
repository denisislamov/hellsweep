#define ENABLE_COLLISION_DRAWING

using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
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
			var positionPool = pools.PositionPool;
			var collisionPool = pools.CollisionPool;

			for (var i = 0; i < _kdTreeStorages.Length; i++)
			{
				var kdTreeStorage = _kdTreeStorages[i];

				DrawCollisionArea(collisionAreaConfig, position, Color.blue);

				if (!DoesLayerMaskContainsLayer(layerMask, kdTreeStorage.Layer))
				{
					continue;
				}

				if (collisionAreaConfig is ICircleCollisionAreaConfig circleCollisionAreaConfig)
				{
					query.Radius(kdTreeStorage.KdTree, position, circleCollisionAreaConfig.Radius * 3, _results);
					
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

						if (!DoesLayerMaskContainsLayer(layerMask, layer.Layer))
						{
							continue;
						}

						if (!collisionPool.Has(entityId) || !positionPool.Has(entityId))
						{
							continue;
						}

						ref var collision = ref collisionPool.Get(entityId);
						ref var entityPosition = ref positionPool.Get(entityId);

						if (!IsIntersected(collisionAreaConfig, collision.CollisionAreaConfig, position, entityPosition.Position))
						{
							continue;
						}
						
						entities.Add(entityId);
					}
				}
				else if (collisionAreaConfig is IAABBCollisionAreaConfig aabbCollisionAreaConfig)
				{
					var sourceRect = aabbCollisionAreaConfig.Rect;
					var rect = new Rect(position - sourceRect.center, sourceRect.size);
					var radius = Mathf.Max(sourceRect.max.magnitude, sourceRect.min.magnitude);

					query.Radius(kdTreeStorage.KdTree, position, radius * 2, _results);
					
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

						if (!DoesLayerMaskContainsLayer(layerMask, layer.Layer))
						{
							entities.Add(entityId);
						}
						
						if (!collisionPool.Has(entityId) || !positionPool.Has(entityId))
						{
							continue;
						}

						ref var collision = ref collisionPool.Get(entityId);
						ref var entityPosition = ref positionPool.Get(entityId);

						if (IsIntersected(collisionAreaConfig, collision.CollisionAreaConfig, position, entityPosition.Position))
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

		private bool IsIntersected(
			ICollisionAreaConfig areaA, 
			ICollisionAreaConfig areaB, 
			Vector2 positionA, 
			Vector2 positionB)
		{
			var circleA = areaA as ICircleCollisionAreaConfig;
			var circleB = areaB as ICircleCollisionAreaConfig;
			
			var aabbA = areaA as IAABBCollisionAreaConfig;
			var aabbB = areaB as IAABBCollisionAreaConfig;

			if (circleA != null && 
				circleB != null)
			{
				var sqrMagnitude = (positionA - positionB).sqrMagnitude;
				var sqrMaxDistance = Mathf.Pow(circleA.Radius + circleB.Radius, 2);

				var result = sqrMagnitude < sqrMaxDistance;

				DrawCircle(circleB, positionB, !result ? Color.green : Color.red);

				return result;
			}
			
			if (circleA != null && 
				aabbB != null)
			{
				var rect = new Rect(
					positionB, 
					new Vector2(
						aabbB.Rect.size.x + 2 * circleA.Radius, 
						aabbB.Rect.size.y + 2 * circleA.Radius)
				);
				
				var result = RectContains(rect, positionB, positionA);
				
				DrawRect(aabbB.Rect, positionB, !result ? Color.green : Color.red);
				DrawCircle(circleA, positionA, !result ? Color.green : Color.red);

				return result;
			}
			
			if (circleB != null && 
				aabbA != null)
			{
				var rect = new Rect(
					positionA, 
					new Vector2(
						aabbA.Rect.size.x + 2 * circleB.Radius, 
						aabbA.Rect.size.y + 2 * circleB.Radius)
				);
				
				var result = RectContains(rect, positionA, positionB);

				DrawRect(aabbA.Rect, positionA, !result ? Color.green : Color.red);
				DrawCircle(circleB, positionB, !result ? Color.green : Color.red);

				return result;
			}
			
			if (aabbA != null && aabbB != null)
			{
				var aRect = new Rect(positionA, aabbA.Rect.size);
				var bRect = new Rect(positionB, aabbB.Rect.size);
				
				var result = aRect.Overlaps(bRect);
				
				DrawRect(bRect, bRect.position, !result ? Color.green : Color.red);
				
				return result;
			}

			return false;
		}

		private bool RectContains(Rect rect, Vector2 rectPosition, Vector2 dotPosition)
		{
			var xMin = rectPosition.x + rect.size.x*-0.5f;
			var xMax = rectPosition.x + rect.size.x*0.5f;
			var yMin = rectPosition.y + rect.size.y*-0.5f;
			var yMax = rectPosition.y + rect.size.y*0.5f;

			return dotPosition.x >= xMin && dotPosition.x <= xMax &&
				   dotPosition.y >= yMin && dotPosition.y <= yMax;
		}
		
		private void DrawCollisionArea(ICollisionAreaConfig collisionAreaConfig, Vector2 position, Color color)
		{
#if ENABLE_COLLISION_DRAWING
			switch (collisionAreaConfig)
			{
				case IAABBCollisionAreaConfig aabbCollisionAreaConfig:
					DrawRect(aabbCollisionAreaConfig.Rect, position, color);
					return;
				case ICircleCollisionAreaConfig circleCollisionAreaConfig:
					DrawCircle(circleCollisionAreaConfig, position, color);
					return;
			}
#endif
		}

		private void DrawRect(Rect rect, Vector2 position, Color color)
		{
#if ENABLE_COLLISION_DRAWING
			var leftDownCorner = new Vector2(position.x + rect.size.x*-0.5f, position.y + rect.size.y*-0.5f);
			var leftUpCorner = new Vector2(position.x + rect.size.x*-0.5f, position.y + rect.size.y*0.5f);
			var rightDownCorner = new Vector2(position.x + rect.size.x*0.5f, position.y + rect.size.y*-0.5f);
			var rightUpCorner = new Vector2(position.x + rect.size.x*0.5f, position.y + rect.size.y*0.5f);
				
			Debug.DrawLine(leftUpCorner, leftDownCorner, color, Time.deltaTime);
			Debug.DrawLine(leftDownCorner, rightDownCorner, color, Time.deltaTime);
			Debug.DrawLine(rightDownCorner, rightUpCorner, color, Time.deltaTime);
			Debug.DrawLine(rightUpCorner, leftUpCorner, color, Time.deltaTime);
#endif
		}
		
		private void DrawCircle(ICircleCollisionAreaConfig circle, Vector2 position, Color color)
		{
#if ENABLE_COLLISION_DRAWING
			var previousDir = Vector2.right;
			var angle = 360/12;
			for (var i = 0; i < 12; i++)
			{
				var dir = previousDir.Rotate(angle);
				
				Debug.DrawLine(position + previousDir * circle.Radius, position + dir * circle.Radius, color, Time.deltaTime);
				
				previousDir = dir;
			}
#endif
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