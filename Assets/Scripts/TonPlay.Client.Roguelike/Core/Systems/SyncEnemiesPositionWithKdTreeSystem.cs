using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncEnemiesPositionWithKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _storage;

		private Vector3[] _positions;
		private bool _inited;

		public SyncEnemiesPositionWithKdTreeSystem(KdTreeStorage storage)
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
			
			for (var i = 0; i < _storage.KdTreePositionIndexToEntityIdMap.Length; i++) 
			{
				var entityId = _storage.KdTreePositionIndexToEntityIdMap[i];
				
				if (entityId == EcsEntity.DEFAULT_ID) continue;

				tree.Points[i] = GetActualPosition(world, entityId);
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