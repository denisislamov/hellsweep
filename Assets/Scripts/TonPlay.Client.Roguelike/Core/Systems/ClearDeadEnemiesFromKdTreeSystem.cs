using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearDeadEnemiesFromKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		public ClearDeadEnemiesFromKdTreeSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin

			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);

#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<EnemyComponent>().Inc<DeadComponent>().End();

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