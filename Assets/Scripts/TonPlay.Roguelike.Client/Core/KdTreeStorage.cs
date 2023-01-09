using DataStructures.ViliWonka.KDTree;

namespace TonPlay.Roguelike.Client.Core
{
	public class KdTreeStorage
	{
		private readonly KDTree _kdTree = new KDTree();

		private readonly KDQuery _kdQuery = new KDQuery();

		private int[] _kdTreePositionIndexToEntityIdMap;
		
		public KDTree KdTree => _kdTree;

		public KDQuery KdQuery => _kdQuery;

		public int[] KdTreePositionIndexToEntityIdMap => _kdTreePositionIndexToEntityIdMap;
		
		public void CreateEnemiesKdTreeIndexToEntityIdMap(int count)
		{
			_kdTreePositionIndexToEntityIdMap = new int[count];
		}
	}
}