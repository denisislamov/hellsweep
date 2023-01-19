using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class BasicEnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		private ICompositeViewPool _pool;
		private ILevelWaveConfigProvider _wavesConfigProvider;
		private IEnemyConfigProvider _enemyConfigProvider;
		private ISharedData _sharedData;

		public BasicEnemySpawnSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			_pool = sharedData.CompositeViewPool;
			_wavesConfigProvider = sharedData.WavesConfigProvider;
			_enemyConfigProvider = sharedData.EnemyConfigProvider;

			var allWaves = _wavesConfigProvider.AllWaves;
			var totalEnemies = 0;

			var maxSpawnedQuantityPerPrefab = new Dictionary<EnemyView, int>();

			foreach (var waveConfig in allWaves)
			{
				if (string.IsNullOrEmpty(waveConfig.EnemyId))
				{
					continue;
				}

				var enemyId = waveConfig.EnemyId;
				var config = sharedData.EnemyConfigProvider.Get(enemyId);

				if (config is null)
				{
					Debug.LogWarning($"Requested enemy with id '{enemyId}' hasn't been found!");
					continue;
				}

				Assert.IsNotNull(config.Prefab);

				if (!maxSpawnedQuantityPerPrefab.ContainsKey(config.Prefab))
				{
					maxSpawnedQuantityPerPrefab.Add(config.Prefab, 0);
				}

				maxSpawnedQuantityPerPrefab[config.Prefab] += waveConfig.MaxSpawnedQuantity;
				totalEnemies += waveConfig.MaxSpawnedQuantity;
			}

			foreach (var kvp in maxSpawnedQuantityPerPrefab)
			{
				_pool.Add(new EnemyViewPoolIdentity(kvp.Key), kvp.Key, kvp.Value);
			}

			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(totalEnemies);
			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(totalEnemies);
			
			_kdTreeStorage.KdTree.Build(new Vector3[totalEnemies]);

			_sharedData = sharedData;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var gameFilter = world.Filter<GameComponent>().Inc<GameTimeComponent>().Inc<WavesDataComponent>().End();
			var timePool = world.GetPool<GameTimeComponent>();
			var deadEnemiesPool = world.GetPool<WavesDataComponent>();

			foreach (var gameEntityId in gameFilter)
			{
				ref var time = ref timePool.Get(gameEntityId);
				ref var wavesData = ref deadEnemiesPool.Get(gameEntityId);

				var timespan = TimeSpan.FromSeconds(time.Time);

				var currentWavesConfigs = _wavesConfigProvider.Get(timespan.Ticks);
				if (currentWavesConfigs == null)
				{
					break;
				}

				foreach (var waveConfig in currentWavesConfigs)
				{
					wavesData.WavesEnemiesKilledAmount ??= new Dictionary<string, int>();
					wavesData.WavesEnemiesSpawnedAmount ??= new Dictionary<string, int>();
					wavesData.WavesTimeLeftToNextSpawn ??= new Dictionary<string, long>();

					if (!wavesData.WavesEnemiesKilledAmount.ContainsKey(waveConfig.Id))
					{
						wavesData.WavesEnemiesKilledAmount.Add(waveConfig.Id, 0);
					}

					if (!wavesData.WavesEnemiesSpawnedAmount.ContainsKey(waveConfig.Id))
					{
						wavesData.WavesEnemiesSpawnedAmount.Add(waveConfig.Id, 0);
					}

					if (!wavesData.WavesTimeLeftToNextSpawn.ContainsKey(waveConfig.Id))
					{
						wavesData.WavesTimeLeftToNextSpawn.Add(waveConfig.Id, 0);
					}

					wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id] -= TimeSpan.FromSeconds(Time.deltaTime).Ticks;

					var timeLeft = wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id];
					if (timeLeft > 0)
					{
						continue;
					}

					var leftEnemiesAmount = waveConfig.EnemiesQuantity - wavesData.WavesEnemiesKilledAmount[waveConfig.Id];
					var maxEnemiesSpawnedQuantity = Math.Min(leftEnemiesAmount, waveConfig.MaxSpawnedQuantity);
					var spawnQuantity = maxEnemiesSpawnedQuantity - wavesData.WavesEnemiesSpawnedAmount[waveConfig.Id];
					spawnQuantity = Mathf.Clamp(spawnQuantity, 0, waveConfig.SpawnQuantityPerRate);

					for (var i = 0; i < spawnQuantity; i++)
					{
						var enemyConfig = _enemyConfigProvider.Get(waveConfig.EnemyId);

						CreateEnemy(world, _sharedData.PlayerPositionProvider.Position, enemyConfig, waveConfig);

						wavesData.WavesEnemiesSpawnedAmount[waveConfig.Id]++;
					}

					wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id] = waveConfig.SpawnTickRate;
				}
			}
		}

		private void CreateEnemy(EcsWorld world, Vector2 playerPosition, IEnemyConfig enemyConfig, IWaveConfig waveConfig)
		{
			var entity = world.NewEntity();

			if (!_pool.TryGet<EnemyView>(new EnemyViewPoolIdentity(enemyConfig.Prefab), out var poolObject))
			{
				return;
			}

			var enemyView = poolObject.Object;
			var randomPosition = CreateRandomPosition(playerPosition);

			var gameObject = enemyView.gameObject;
			gameObject.name = string.Format("{0} (entity: {1})", enemyConfig.Prefab.gameObject.name, entity.Id.ToString());

			enemyView.transform.position = randomPosition;

			var freeTreeIndex = FindFreeTreeIndex();

			_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Add(entity.Id, freeTreeIndex);
			_kdTreeStorage.KdTreePositionIndexToEntityIdMap[freeTreeIndex] = entity.Id;
			_kdTreeStorage.KdTree.Points[freeTreeIndex] = enemyView.transform.position;

			enemyView.SetEntityId(entity.Id);

			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = enemyView.gameObject;

			AddEnemyComponent(entity, enemyConfig, waveConfig);
			AddMovementComponent(entity);
			AddLayerComponent(entity, enemyView);
			AddCollidersComponent(entity, enemyView);
			AddTransformComponent(entity, enemyView);
			AddPositionComponent(entity, randomPosition);

			var lerpTransformComponent = AddLerpTransformComponent(entity);

			AddSpeedComponent(entity, enemyConfig.MovementConfig);
			AddHealthComponent(entity, enemyConfig);
			AddDamageOnCollisionComponent(entity, enemyConfig);

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

		private void AddMovementComponent(EcsEntity entity)
		{
			entity.Add<MovementComponent>();
		}

		private static void AddEnemyComponent(EcsEntity entity, IEnemyConfig enemyConfig, IWaveConfig waveConfig)
		{
			ref var enemy = ref entity.Add<EnemyComponent>();
			enemy.ConfigId = enemyConfig.Id;
			enemy.WaveId = waveConfig.Id;
		}

		private void AddLayerComponent(EcsEntity entity, EnemyView enemyView)
		{
			ref var layer = ref entity.Add<LayerComponent>();
			layer.Layer = enemyView.gameObject.layer;
		}

		private static Vector2 CreateRandomPosition(Vector2 playerPosition)
		{
			var randomPosition = Random.insideUnitCircle*45f;
			var distanceToPlayer = Vector2.Distance(randomPosition, playerPosition);

			if (distanceToPlayer < 15f)
			{
				var direction = (randomPosition - playerPosition).normalized;
				randomPosition = playerPosition + direction*Random.Range(15f, 45f);
			}
			return randomPosition;
		}

		private static void AddDamageOnCollisionComponent(EcsEntity entity, IEnemyConfig config)
		{
			ref var damageOnCollisionComponent = ref entity.Add<DamageOnCollisionComponent>();
			damageOnCollisionComponent.Damage = config.DamageOnCollision;
		}

		private static void AddHealthComponent(EcsEntity entity, IEnemyConfig config)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = config.StartHealth;
			healthComponent.MaxHealth = config.StartHealth;
		}

		private static void AddSpeedComponent(EcsEntity entity, IMovementConfig movementConfig)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = movementConfig.StartSpeed;
		}

		private static LerpTransformComponent AddLerpTransformComponent(EcsEntity entity)
		{
			ref var lerpTransformComponent = ref entity.Add<LerpTransformComponent>();
			lerpTransformComponent.ValueToInterpolate = 0.3f;
			return lerpTransformComponent;
		}

		private static void AddPositionComponent(EcsEntity entity, Vector2 randomPosition)
		{
			ref var positionComponent = ref entity.Add<PositionComponent>();
			positionComponent.Position = randomPosition;
		}

		private static void AddTransformComponent(EcsEntity entity, EnemyView enemy)
		{
			ref var transformComponent = ref entity.Add<TransformComponent>();
			transformComponent.Transform = enemy.transform;
		}

		private static void AddCollidersComponent(EcsEntity entity, EnemyView enemy)
		{
			ref var collidersComponent = ref entity.Add<CollidersComponent>();
			collidersComponent.AttachedColliders = new Collider2D[1] {enemy.Collider2D};
		}

		private static void AddPoolObjectComponent(EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var poolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			poolObjectComponent.ViewPoolObject = viewPoolObject;
		}
	}
}