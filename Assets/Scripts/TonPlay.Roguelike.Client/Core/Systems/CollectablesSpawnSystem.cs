using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class CollectablesSpawnSystem : IEcsInitSystem
	{
		private const int POOL_SIZE = 512;
		
		private readonly KdTreeStorage _kdTreeStorage;

		private ICompositeViewPool _pool;
		private ICollectableConfigProvider _collectablesConfigProvider;
		private ISharedData _sharedData;

		public CollectablesSpawnSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();
 
			_pool = sharedData.CompositeViewPool;
			_collectablesConfigProvider = sharedData.CollectablesConfigProvider;

			var maxSpawnedQuantityPerPrefab = new Dictionary<CollectableView, int>();
			var total = 0;

			foreach (var config in _collectablesConfigProvider.AllCollectables)
			{
				maxSpawnedQuantityPerPrefab.Add(config.Prefab, POOL_SIZE);

				total += POOL_SIZE;
			}

			foreach (var kvp in maxSpawnedQuantityPerPrefab)
			{
				_pool.Add(new CollectableViewPoolIdentity(kvp.Key), kvp.Key, kvp.Value);
			}

			_kdTreeStorage.CreateEnemiesKdTreeIndexToEntityIdMap(total);
			_kdTreeStorage.CreateEnemiesEntityIdToKdTreeIndexMap(total);
			
			_kdTreeStorage.KdTree.Build(new Vector3[total]);

			_sharedData = sharedData;
		}
	}
}