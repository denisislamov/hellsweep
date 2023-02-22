using System;
using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core
{
	public class KdTreeStorage
	{
		private readonly KDTree _kdTree = new KDTree();

		private readonly KDQuery _kdQuery = new KDQuery();

		private readonly int _layer;

		private int[] _kdTreePositionIndexToEntityIdMap;
		private Dictionary<int, int> _kdTreeEntityIdToPositionIndexMap;

		public int Layer => _layer;
		
		public KDTree KdTree => _kdTree;

		public KDQuery KdQuery => _kdQuery;

		public int[] KdTreePositionIndexToEntityIdMap => _kdTreePositionIndexToEntityIdMap;
		
		public Dictionary<int, int> KdTreeEntityIdToPositionIndexMap => _kdTreeEntityIdToPositionIndexMap;

		public bool Changed => _changed;
		
		private bool _changed;

		public KdTreeStorage(int layer)
		{
			_layer = layer;
		}
		
		public void CreateEntityIdToKdTreeIndexMap(int count)
		{
			_kdTreeEntityIdToPositionIndexMap = new Dictionary<int, int>(count);
			
			_changed = true;
		}
		
		public void CreateKdTreeIndexToEntityIdMap(int count)
		{
			_kdTreePositionIndexToEntityIdMap = new int[count];
			
			for (int i = 0; i < count; i++)
			{
				_kdTreePositionIndexToEntityIdMap[i] = EcsEntity.DEFAULT_ID;
			}
			
			_changed = true;
		}

		public int AddEntity(int entityId, Vector3 position)
		{
			var freeTreeIndex = FindFreeTreeIndex();

			if (KdTreeEntityIdToPositionIndexMap.ContainsKey(entityId))
			{
				var deadEntityTreeIndex = KdTreeEntityIdToPositionIndexMap[entityId];
				KdTreePositionIndexToEntityIdMap[deadEntityTreeIndex] = EcsEntity.DEFAULT_ID;
				KdTreeEntityIdToPositionIndexMap.Remove(entityId);
			}

			if (freeTreeIndex == -1)
			{
				// var leakedEntities = new List<int>();
				// var hashSet = new HashSet<int>();
				//
				// for (int i = 0; i < KdTreePositionIndexToEntityIdMap.Length; i++)
				// {
				// 	var entity = KdTreePositionIndexToEntityIdMap[i];
				// 	if (!hashSet.Contains(entity))
				// 	{
				// 		hashSet.Add(entity);
				// 	}
				// 	else
				// 	{
				// 		leakedEntities.Add(entity);
				// 	}
				// }
				//
				// throw new NotSupportedException();

				var previousSize = _kdTreePositionIndexToEntityIdMap.Length;
				
				Array.Resize(ref _kdTreePositionIndexToEntityIdMap, previousSize * 2);

				for (int i = previousSize - 1; i < _kdTreePositionIndexToEntityIdMap.Length; i++)
				{
					_kdTreePositionIndexToEntityIdMap[i] = -1;
				}
				
				_kdTree.SetCount(_kdTreePositionIndexToEntityIdMap.Length);
				
				Debug.LogWarning("Resize KD Tree Storage!");
				
				freeTreeIndex = FindFreeTreeIndex();
			}
			
			KdTreeEntityIdToPositionIndexMap.Add(entityId, freeTreeIndex);
			KdTreePositionIndexToEntityIdMap[freeTreeIndex] = entityId;
			KdTree.Points[freeTreeIndex] = position;
			
			_changed = true;

			return freeTreeIndex;
		}
		
		public void RemoveEntity(int entityId)
		{
			if (!KdTreeEntityIdToPositionIndexMap.ContainsKey(entityId))
			{
				return;
			}
				
			var treeIndex = KdTreeEntityIdToPositionIndexMap[entityId];
			KdTreePositionIndexToEntityIdMap[treeIndex] = EcsEntity.DEFAULT_ID;
			KdTreeEntityIdToPositionIndexMap.Remove(entityId);
			
			_changed = true;
		}
		
		public void UpdateElement(int entityId, Vector2 positionPosition)
		{
			var treeIndex = KdTreeEntityIdToPositionIndexMap[entityId];
			KdTree.Points[treeIndex] = positionPosition;
			
			_changed = true;
		}
		
		public void Rebuild()
		{
			KdTree.Rebuild();

			_changed = false;
		}
		
		private int FindFreeTreeIndex()
		{
			for (var i = 0; i < KdTreePositionIndexToEntityIdMap.Length; i++)
			{
				var entityId = KdTreePositionIndexToEntityIdMap[i];
				if (KdTreeEntityIdToPositionIndexMap.ContainsKey(entityId))
				{
					continue;
				}

				return i;
			}
			
			return -1;
		}
	}
}