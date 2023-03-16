using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CollectablesSpawnSystem : IEcsInitSystem
	{
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
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();

			_pool = sharedData.CompositeViewPool;
			_collectablesConfigProvider = sharedData.CollectablesConfigProvider;

			var maxSpawnedQuantityPerPrefab = new Dictionary<CollectableView, int>();
			var total = 0;

			foreach (var config in _collectablesConfigProvider.AllCollectables)
			{
				if (!maxSpawnedQuantityPerPrefab.ContainsKey(config.Prefab))
				{
					maxSpawnedQuantityPerPrefab.Add(config.Prefab, 0);
				}

				maxSpawnedQuantityPerPrefab[config.Prefab] += config.PoolSize;

				total += config.PoolSize;
			}

			foreach (var kvp in maxSpawnedQuantityPerPrefab)
			{
				_pool.Add(new CollectableViewPoolIdentity(kvp.Key), kvp.Key, kvp.Value);
			}

			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(total);
			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(total);

			_kdTreeStorage.KdTree.Build(new Vector2[total]);

			_sharedData = sharedData;
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}