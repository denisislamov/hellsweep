using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CollectablesSpawnOnEnemyDiedEventSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		private ICompositeViewPool _pool;
		private ISharedData _sharedData;

		public CollectablesSpawnOnEnemyDiedEventSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();
			_pool = _sharedData.CompositeViewPool;
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var diedEnemyPool = world.GetPool<EnemyDiedEvent>();

			var filter = world.Filter<EnemyDiedEvent>().End();

			foreach (var entityId in filter)
			{
				ref var diedEnemy = ref diedEnemyPool.Get(entityId);

				var collectables = diedEnemy.EnemyConfig.CollectablesIdsOnDeath;

				if (collectables == null || collectables.Count == 0)
				{
					world.DelEntity(entityId);
					continue;
				}

				foreach (var collectableId in collectables)
				{
					var collectableConfig = _sharedData.CollectablesConfigProvider.Get(collectableId);
					
					CreateExperienceCollectable(world, diedEnemy.Position, collectableConfig);
				}
				
				diedEnemyPool.Del(entityId);
				
				world.DelEntity(entityId);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}

		private void CreateExperienceCollectable(EcsWorld world, Vector2 position, ICollectableConfig collectableConfig)
		{
			var entity = world.NewEntity();

			if (!_pool.TryGet<CollectableView>(new CollectableViewPoolIdentity(collectableConfig.Prefab), out var poolObject))
			{
				return;
			}

			var enemyView = poolObject.Object;

			var gameObject = enemyView.gameObject;
			gameObject.name = string.Format("{0} (entity: {1})", collectableConfig.Prefab.gameObject.name, entity.Id.ToString());

			enemyView.transform.position = position;

			var freeTreeIndex = FindFreeTreeIndex();

			_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Add(entity.Id, freeTreeIndex);
			_kdTreeStorage.KdTreePositionIndexToEntityIdMap[freeTreeIndex] = entity.Id;
			_kdTreeStorage.KdTree.Points[freeTreeIndex] = enemyView.transform.position;

			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = enemyView.gameObject;

			AddCollectableComponent(entity, collectableConfig);
			AddLayerComponent(entity, enemyView);
			AddPositionComponent(entity, position);

			AddPoolObjectComponent(entity, poolObject);
		}
		private int FindFreeTreeIndex()
		{
			var freeTreeIndex = -1;
			for (var i = 0; i < _kdTreeStorage.KdTreePositionIndexToEntityIdMap.Length; i++)
			{
				if (_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.ContainsKey(_kdTreeStorage.KdTreePositionIndexToEntityIdMap[i]))
				{
					continue;
				}

				freeTreeIndex = i;

				break;
			}
			return freeTreeIndex;
		}

		private static void AddCollectableComponent(EcsEntity entity, ICollectableConfig config)
		{
			ref var collectable = ref entity.Add<CollectableComponent>();
			collectable.Type = config.Type;
			collectable.Value = config.Value;
		}

		private void AddLayerComponent(EcsEntity entity, CollectableView collectableView)
		{
			ref var layer = ref entity.Add<LayerComponent>();
			layer.Layer = collectableView.gameObject.layer;
		}

		private static void AddPositionComponent(EcsEntity entity, Vector2 randomPosition)
		{
			ref var positionComponent = ref entity.Add<PositionComponent>();
			positionComponent.Position = randomPosition;
		}

		private static void AddPoolObjectComponent(EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var poolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			poolObjectComponent.ViewPoolObject = viewPoolObject;
		}
	}
}