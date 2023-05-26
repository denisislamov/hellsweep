using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
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
			ICollisionArea collisionArea,
			ref List<int> entities,
			int layerMask,
			IOverlapParams overlapParams)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.Overlap");

			for (var i = 0; i < _kdTreeStorages.Length; i++)
			{
				var kdTreeStorage = _kdTreeStorages[i];

				if (!LayerMaskExt.ContainsLayer(layerMask, kdTreeStorage.Layer))
				{
					continue;
				}
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawCollisionArea(collisionArea, position, Color.blue);
#endif
				//query.KNearest(kdTreeStorage.KdTree, position, RoguelikeConstants.Core.Collision.OVERLAP_K_NEAREST_COUNT, _results);

				var radius = GetOverlapRadius(collisionArea);

				query.Radius(kdTreeStorage.KdTree, position, radius * radius + RoguelikeConstants.Core.Collision.OVERLAP_MIN_RADIUS, _results);

				for (var j = 0; j < _results.Count; j++)
				{
					var index = _results[j];
					var entityId = kdTreeStorage.KdTreePositionIndexToEntityIdMap[index];

					// if (!EntityIsAlive(entityId))
					// 	continue;

					if (!IsFilteredEntity(overlapParams, entityId))
						continue;

					// ref var layer = ref overlapParams.LayerPool.Get(entityId);
					//
					// if (!LayerMaskExt.ContainsLayer(layerMask, layer.Layer))
					// {
					// 	continue;
					// }

					ref var collision = ref overlapParams.CollisionPool.Get(entityId);
					var entityPosition = kdTreeStorage.KdTree.Points[index];

					if (IsIntersected(collisionArea, collision.CollisionArea, position, entityPosition))
					{
						entities.Add(entityId);
					}
				}

				_results.Clear();
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();

			return entities.Count;
		}
		
		private static float GetOverlapRadius(ICollisionArea collisionArea)
		{
			var radius = 0f;

			switch (collisionArea.Config)
			{
				case ICircleCollisionAreaConfig circleCollisionAreaConfig:
				{
					radius = circleCollisionAreaConfig.Radius * collisionArea.Scale;
					break;
				}
				case IAABBCollisionAreaConfig aabbCollisionAreaConfig:
				{
					var sourceRect = aabbCollisionAreaConfig.Rect;
					radius = Mathf.Max(sourceRect.max.magnitude, sourceRect.min.magnitude) * collisionArea.Scale;
					break;
				}
				case ICompositeCollisionAreaConfig compositeCollisionAreaConfig 
					when collisionArea is ICompositeCollisionArea compositeCollisionArea:
				{
					var maxRadius = 0f;
					for (var index = 0; index < compositeCollisionAreaConfig.CollisionAreaConfigs.Count; index++)
					{
						var innerArea = compositeCollisionArea.CollisionAreas[index];
						var distanceToInnerConfig = (innerArea.Position - compositeCollisionAreaConfig.Position).SqrMagnitude(); 
						var innerConfigRadius = GetOverlapRadius(innerArea) + distanceToInnerConfig;

						if (maxRadius < innerConfigRadius)
						{
							maxRadius = innerConfigRadius;
						}
					}
					radius = maxRadius;
					break;
				}
			}
			
			return radius;
		}

		private bool IsFilteredEntity(IOverlapParams overlapParams, int entityId)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.IsFilteredEntity");

			var isFilteredEntity = overlapParams.FilteredEntities.Contains(entityId);
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
			return isFilteredEntity;
		}

		private bool EntityIsAlive(int entityId)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.EntityIsAlive");

			if (entityId == EcsEntity.DEFAULT_ID || !_world.IsEntityAlive(entityId))
			{
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return false;
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
			return true;
		}

		private static bool CheckForComponents(EcsPool<LayerComponent> layerPool, int entityId, EcsPool<DestroyComponent> destroyPool, EcsPool<InactiveComponent> inactivePool, EcsPool<DeadComponent> deadPool, EcsPool<UsedComponent> usedPool)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.CheckForComponents");
			if (!layerPool.Has(entityId) ||
				destroyPool.Has(entityId) ||
				inactivePool.Has(entityId) ||
				deadPool.Has(entityId) ||
				usedPool.Has(entityId))
			{
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return true;
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
			return false;
		}

		/*private bool IsIntersected(
			ICollisionAreaConfig area, 
			Vector2 positionA,
			Vector2 positionB)
		{
			var circle = area as ICircleCollisionAreaConfig;
			var aabb = area as IAABBCollisionAreaConfig;
	
			if (circle != null)
			{
				return true;
			}
			
			if (aabb != null)
			{
				return RectContains(aabb.Rect, positionA, positionB);
			}
			
			return false;
		}*/

		private bool IsIntersected(
			ICollisionArea areaA,
			ICollisionArea areaB,
			Vector2 positionA,
			Vector2 positionB)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.IsIntersected");

			var circleA = areaA.Config as ICircleCollisionAreaConfig;
			var circleB = areaB.Config as ICircleCollisionAreaConfig;

			var aabbA = areaA.Config as IAABBCollisionAreaConfig;
			var aabbB = areaB.Config as IAABBCollisionAreaConfig;

			var compositeA = areaA as ICompositeCollisionArea;
			var compositeB = areaB as ICompositeCollisionArea;

			if (circleA != null &&
				circleB != null)
			{
				var diffX = positionB.x - positionA.x;
				var diffY = positionB.y - positionA.y;
				var sqrMagnitude = diffX*diffX + diffY*diffY;
				var combinedRadius = circleA.Radius * areaA.Scale + circleB.Radius * areaB.Scale;
				var sqrMaxDistance = combinedRadius * combinedRadius;

				var result = sqrMagnitude < sqrMaxDistance;
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawCircle(circleB, positionB + circleB.Position, !result ? Color.green : Color.red, areaB.Scale);
#endif

				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return result;
			}

			if (circleA != null &&
				aabbB != null)
			{
				var rect = new Rect(
					positionB,
					new Vector2(
						aabbB.Rect.size.x * areaB.Scale + 2 * circleA.Radius * areaA.Scale,
						aabbB.Rect.size.y * areaB.Scale + 2 * circleA.Radius * areaA.Scale)
				);

				var result = RectContains(rect, positionB + aabbB.Position, positionA + circleA.Position);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(aabbB.Rect, positionB + aabbB.Position, !result ? Color.green : Color.red, areaB.Scale);
				DrawCircle(circleA, positionA + circleA.Position, !result ? Color.green : Color.red, areaA.Scale);
#endif
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return result;
			}

			if (circleB != null &&
				aabbA != null)
			{
				var rect = new Rect(
					positionA,
					new Vector2(
						aabbA.Rect.size.x * areaA.Scale + 2*circleB.Radius * areaB.Scale,
						aabbA.Rect.size.y * areaA.Scale + 2*circleB.Radius * areaB.Scale)
				);

				var result = RectContains(rect, positionA + aabbA.Position, positionB + circleB.Position);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(aabbA.Rect, positionA + aabbA.Position, !result ? Color.green : Color.red, areaA.Scale);
				DrawCircle(circleB, positionB + circleB.Position, !result ? Color.green : Color.red, areaB.Scale);
#endif
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return result;
			}

			if (aabbA != null && aabbB != null)
			{
				var aRect = new Rect(positionA + aabbA.Position, aabbA.Rect.size * areaA.Scale);
				var bRect = new Rect(positionB + aabbB.Position, aabbB.Rect.size * areaB.Scale);

				var result = aRect.Overlaps(bRect);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(bRect, bRect.position, !result ? Color.green : Color.red, areaB.Scale);
#endif
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return result;
			}

			if (compositeA != null)
			{
				for (var i = 0; i < compositeA.CollisionAreas.Count; i++)
				{
					var innerAreaA = compositeA.CollisionAreas[i];
					
					if (IsIntersected(innerAreaA, areaB, positionA, positionB))
						return true;
				}

				return false;
			}
			
			if (compositeB != null)
			{
				for (var i = 0; i < compositeB.CollisionAreas.Count; i++)
				{
					var innerAreaB = compositeB.CollisionAreas[i];
					
					if (IsIntersected(areaA, innerAreaB, positionA, positionB))
						return true;
				}

				return false;
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

#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
		private void DrawCollisionArea(ICollisionArea collisionArea, Vector2 position, Color color)
		{
			switch (collisionArea.Config)
			{
				case IAABBCollisionAreaConfig aabbCollisionAreaConfig:
					DrawRect(aabbCollisionAreaConfig.Rect, position + aabbCollisionAreaConfig.Position, color, collisionArea.Scale);
					return;
				case ICircleCollisionAreaConfig circleCollisionAreaConfig:
					DrawCircle(circleCollisionAreaConfig, position + circleCollisionAreaConfig.Position, color, collisionArea.Scale);
					return;
				case ICompositeCollisionAreaConfig compositeCollisionAreaConfig 
					when collisionArea is ICompositeCollisionArea compositeCollisionArea:
				{
					for (var i = 0; i < compositeCollisionArea.CollisionAreas.Count; i++)
					{
						var area = compositeCollisionArea.CollisionAreas[i];
						DrawCollisionArea(area, position + compositeCollisionAreaConfig.Position, color);
					}
					return;
				}
			}

		}

		private void DrawRect(Rect rect, Vector2 position, Color color, float scale)
		{
			var leftDownCorner = new Vector2(position.x + rect.size.x*scale*-0.5f, position.y + rect.size.y*scale*-0.5f);
			var leftUpCorner = new Vector2(position.x + rect.size.x*scale*-0.5f, position.y + rect.size.y*scale*0.5f);
			var rightDownCorner = new Vector2(position.x + rect.size.x*scale*0.5f, position.y + rect.size.y*scale*-0.5f);
			var rightUpCorner = new Vector2(position.x + rect.size.x*scale*0.5f, position.y + rect.size.y*scale*0.5f);

			Debug.DrawLine(leftUpCorner, leftDownCorner, color, Time.deltaTime);
			Debug.DrawLine(leftDownCorner, rightDownCorner, color, Time.deltaTime);
			Debug.DrawLine(rightDownCorner, rightUpCorner, color, Time.deltaTime);
			Debug.DrawLine(rightUpCorner, leftUpCorner, color, Time.deltaTime);
		}

		private void DrawCircle(ICircleCollisionAreaConfig circle, Vector2 position, Color color, float scale)
		{
			var previousDir = Vector2.right;
			var angle = 360/12;
			for (var i = 0; i < 12; i++)
			{
				var dir = previousDir.Rotate(angle);

				Debug.DrawLine(position + previousDir*(circle.Radius*scale), position + dir*(circle.Radius*scale), color, Time.deltaTime);

				previousDir = dir;
			}
		}
#endif

		public class Factory : PlaceholderFactory<EcsWorld, KdTreeStorage[], OverlapExecutor>
		{
		}
	}

}