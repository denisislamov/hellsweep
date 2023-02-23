using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
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
				DrawCollisionArea(collisionAreaConfig, position, Color.blue);
#endif
				//query.KNearest(kdTreeStorage.KdTree, position, RoguelikeConstants.Core.Collision.OVERLAP_K_NEAREST_COUNT, _results);

				var radius = 0f;

				switch (collisionAreaConfig)
				{
					case ICircleCollisionAreaConfig circleCollisionAreaConfig:
					{
						radius = circleCollisionAreaConfig.Radius;
						break;
					}
					case IAABBCollisionAreaConfig aabbCollisionAreaConfig:
					{
						var sourceRect = aabbCollisionAreaConfig.Rect;
						radius = Mathf.Max(sourceRect.max.magnitude, sourceRect.min.magnitude);
						break;
					}
				}

				query.Radius(kdTreeStorage.KdTree, position, radius + RoguelikeConstants.Core.Collision.OVERLAP_MIN_RADIUS, _results);

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

					if (IsIntersected(collisionAreaConfig, collision.CollisionAreaConfig, position, entityPosition))
					{
						entities.Add(entityId);
					}
				}

				_results.Clear();
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();

			return entities.Count;
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
			ICollisionAreaConfig areaA,
			ICollisionAreaConfig areaB,
			Vector2 positionA,
			Vector2 positionB)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("OverlapExecutor.IsIntersected");

			var circleA = areaA as ICircleCollisionAreaConfig;
			var circleB = areaB as ICircleCollisionAreaConfig;

			var aabbA = areaA as IAABBCollisionAreaConfig;
			var aabbB = areaB as IAABBCollisionAreaConfig;

			if (circleA != null &&
				circleB != null)
			{
				var diffX = positionB.x - positionA.x;
				var diffY = positionB.y - positionA.y;
				var sqrMagnitude = diffX*diffX + diffY*diffY;
				var combinedRadius = circleA.Radius + circleB.Radius;
				var sqrMaxDistance = combinedRadius*combinedRadius;

				var result = sqrMagnitude < sqrMaxDistance;
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawCircle(circleB, positionB, !result ? Color.green : Color.red);
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
						aabbB.Rect.size.x + 2*circleA.Radius,
						aabbB.Rect.size.y + 2*circleA.Radius)
				);

				var result = RectContains(rect, positionB, positionA);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(aabbB.Rect, positionB, !result ? Color.green : Color.red);
				DrawCircle(circleA, positionA, !result ? Color.green : Color.red);
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
						aabbA.Rect.size.x + 2*circleB.Radius,
						aabbA.Rect.size.y + 2*circleB.Radius)
				);

				var result = RectContains(rect, positionA, positionB);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(aabbA.Rect, positionA, !result ? Color.green : Color.red);
				DrawCircle(circleB, positionB, !result ? Color.green : Color.red);
#endif
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return result;
			}

			if (aabbA != null && aabbB != null)
			{
				var aRect = new Rect(positionA, aabbA.Rect.size);
				var bRect = new Rect(positionB, aabbB.Rect.size);

				var result = aRect.Overlaps(bRect);
#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
				DrawRect(bRect, bRect.position, !result ? Color.green : Color.red);
#endif
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
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

#if UNITY_EDITOR && ENABLE_COLLISION_DRAWING
		private void DrawCollisionArea(ICollisionAreaConfig collisionAreaConfig, Vector2 position, Color color)
		{
			switch (collisionAreaConfig)
			{
				case IAABBCollisionAreaConfig aabbCollisionAreaConfig:
					DrawRect(aabbCollisionAreaConfig.Rect, position, color);
					return;
				case ICircleCollisionAreaConfig circleCollisionAreaConfig:
					DrawCircle(circleCollisionAreaConfig, position, color);
					return;
			}

		}

		private void DrawRect(Rect rect, Vector2 position, Color color)
		{
			var leftDownCorner = new Vector2(position.x + rect.size.x*-0.5f, position.y + rect.size.y*-0.5f);
			var leftUpCorner = new Vector2(position.x + rect.size.x*-0.5f, position.y + rect.size.y*0.5f);
			var rightDownCorner = new Vector2(position.x + rect.size.x*0.5f, position.y + rect.size.y*-0.5f);
			var rightUpCorner = new Vector2(position.x + rect.size.x*0.5f, position.y + rect.size.y*0.5f);

			Debug.DrawLine(leftUpCorner, leftDownCorner, color, Time.deltaTime);
			Debug.DrawLine(leftDownCorner, rightDownCorner, color, Time.deltaTime);
			Debug.DrawLine(rightDownCorner, rightUpCorner, color, Time.deltaTime);
			Debug.DrawLine(rightUpCorner, leftUpCorner, color, Time.deltaTime);
		}

		private void DrawCircle(ICircleCollisionAreaConfig circle, Vector2 position, Color color)
		{
			var previousDir = Vector2.right;
			var angle = 360/12;
			for (var i = 0; i < 12; i++)
			{
				var dir = previousDir.Rotate(angle);

				Debug.DrawLine(position + previousDir*circle.Radius, position + dir*circle.Radius, color, Time.deltaTime);

				previousDir = dir;
			}
		}
#endif

		public class Factory : PlaceholderFactory<EcsWorld, KdTreeStorage[], OverlapExecutor>
		{
		}
	}

}