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
			var positionPool = world.GetPool<PositionComponent>();

			for (var index = 0; index < _storage.KdTreePositionIndexToEntityIdMap.Length; index++)
			{
				var entityId = _storage.KdTreePositionIndexToEntityIdMap[index];

				if (entityId == EcsEntity.DEFAULT_ID || !world.IsEntityAlive(entityId)) continue;

				tree.Points[index] = GetActualPosition(positionPool, entityId);
			}

			tree.Rebuild();
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
		
		private static Vector3 GetActualPosition(EcsPool<PositionComponent> positionPool, int entityId)
		{
			ref var position = ref positionPool.Get(entityId);
			return position.Position;
		}
	}
}