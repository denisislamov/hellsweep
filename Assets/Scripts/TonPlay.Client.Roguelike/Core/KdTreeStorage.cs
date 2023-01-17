using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;

namespace TonPlay.Roguelike.Client.Core
{
	public class KdTreeStorage
	{
		private readonly KDTree _kdTree = new KDTree();

		private readonly KDQuery _kdQuery = new KDQuery();

		private int[] _kdTreePositionIndexToEntityIdMap;
		private Dictionary<int, int> _kdTreeEntityIdToPositionIndexMap;

		public KDTree KdTree => _kdTree;

		public KDQuery KdQuery => _kdQuery;

		public int[] KdTreePositionIndexToEntityIdMap => _kdTreePositionIndexToEntityIdMap;
		public Dictionary<int, int> KdTreeEntityIdToPositionIndexMap => _kdTreeEntityIdToPositionIndexMap;
		
		public void CreateEnemiesEntityIdToKdTreeIndexMap(int count)
		{
			_kdTreeEntityIdToPositionIndexMap = new Dictionary<int, int>(count);
		}
		
		public void CreateEnemiesKdTreeIndexToEntityIdMap(int count)
		{
			_kdTreePositionIndexToEntityIdMap = new int[count];
			
			for (int i = 0; i < count; i++)
			{
				_kdTreePositionIndexToEntityIdMap[i] = EcsEntity.DEFAULT_ID;
			}
		}
	}
}