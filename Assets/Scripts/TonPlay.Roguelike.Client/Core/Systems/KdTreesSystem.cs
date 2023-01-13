using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class KdTreesSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage _storage;

		private Vector3[] _positions;
		private bool _inited;

		public KdTreesSystem(KdTreeStorage storage)
		{
			_storage = storage;
		}

		public void Init(EcsSystems systems)
		{
			// if (_inited) return;
			//
			// var world = systems.GetWorld();
			//
			// var filter = world.Filter<EnemyComponent>()
			// 				  .Inc<PositionComponent>()
			// 				  .End();
			//
			// var positionPool = world.GetPool<PositionComponent>();
			//
			// var count = filter.GetEntitiesCount();
			//
			// _positions = new Vector3[count];
			//
			// var i = 0;
			// foreach (var entityId in filter)
			// {
			// 	ref var position = ref positionPool.Get(entityId);
			// 	
			// 	_positions[i] = position.Position;
			// 	
			// 	_storage.KdTreePositionIndexToEntityIdMap[i] = entityId;
			// 	_storage.KdTreeEntityIdToPositionIndexMap[entityId] = i;
			// 	i++;
			// }
			//
			// _storage.KdTree.Build(_positions);
			// _inited = true;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var tree = _storage.KdTree;
			
			for (var i = 0; i < _storage.KdTreePositionIndexToEntityIdMap.Length; i++) 
			{
				var entityId = _storage.KdTreePositionIndexToEntityIdMap[i];
				
				if (entityId == EcsEntity.DEFAULT_ID) continue;

				tree.Points[i] = GetActualPosition(world, entityId);
			}

			tree.Rebuild();
		}
		
		private static Vector3 GetActualPosition(EcsWorld world, int entityId)
		{
			var positionPool = world.GetPool<PositionComponent>();
			
			ref var position = ref positionPool.Get(entityId);
			return position.Position;
		}
	}
}