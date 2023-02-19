using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;

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

		public KdTreeStorage(int layer)
		{
			_layer = layer;
		}
		
		public void CreateEntityIdToKdTreeIndexMap(int count)
		{
			_kdTreeEntityIdToPositionIndexMap = new Dictionary<int, int>(count);
		}
		
		public void CreateKdTreeIndexToEntityIdMap(int count)
		{
			_kdTreePositionIndexToEntityIdMap = new int[count];
			
			for (int i = 0; i < count; i++)
			{
				_kdTreePositionIndexToEntityIdMap[i] = EcsEntity.DEFAULT_ID;
			}
		}
	}
}