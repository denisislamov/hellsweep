using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncPlayerPositionWithKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _storage;

		private Vector3[] _positions;
		private bool _inited;

		public SyncPlayerPositionWithKdTreeSystem(KdTreeStorage storage)
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
			
			for (var i = 0; i < _storage.KdTreePositionIndexToEntityIdMap.Length; i++) 
			{
				var entityId = _storage.KdTreePositionIndexToEntityIdMap[i];
				
				if (entityId == EcsEntity.DEFAULT_ID || !world.IsEntityAlive(entityId)) continue;

				tree.Points[i] = GetActualPosition(positionPool, entityId);
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