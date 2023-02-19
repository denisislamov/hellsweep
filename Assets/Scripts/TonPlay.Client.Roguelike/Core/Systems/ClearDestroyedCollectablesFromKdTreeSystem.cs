using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ClearDestroyedCollectablesFromKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;
		
		public ClearDestroyedCollectablesFromKdTreeSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<CollectableComponent>().Inc<DestroyComponent>().End();
			
			foreach (var entityId in filter)
			{
				if (!_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.ContainsKey(entityId))
				{
					continue;
				}
				
				var treeIndex = _kdTreeStorage.KdTreeEntityIdToPositionIndexMap[entityId];
				_kdTreeStorage.KdTreePositionIndexToEntityIdMap[treeIndex] = EcsEntity.DEFAULT_ID;
				_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Remove(entityId);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}