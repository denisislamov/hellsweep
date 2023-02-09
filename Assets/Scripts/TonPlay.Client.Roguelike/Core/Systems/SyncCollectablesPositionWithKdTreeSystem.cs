using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncCollectablesPositionWithKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _storage;
		
		public SyncCollectablesPositionWithKdTreeSystem(KdTreeStorage storage)
		{
			_storage = storage;
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var tree = _storage.KdTree;

			foreach (var kvp in _storage.KdTreeEntityIdToPositionIndexMap)
			{
				var entityId = kvp.Key;
				var index = kvp.Value;
				
				if (entityId == EcsEntity.DEFAULT_ID) continue;

				tree.Points[index] = GetActualPosition(world, entityId);
			}

			tree.Rebuild();
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
		
		private static Vector3 GetActualPosition(EcsWorld world, int entityId)
		{
			var positionPool = world.GetPool<PositionComponent>();
			
			ref var position = ref positionPool.Get(entityId);
			return position.Position;
		}
	}
}