using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EnemyWaveSpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int PROJECTILE_COUNT_PER_ENEMY = 8;

		private readonly KdTreeStorage _kdTreeStorage;

		private ICompositeViewPool _pool;
		private ILevelEnemyWaveConfigProvider _enemyWavesConfigProvider;
		private IEnemyConfigProvider _enemyConfigProvider;
		private ISharedData _sharedData;

		public EnemyWaveSpawnSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			_pool = sharedData.CompositeViewPool;
			_enemyWavesConfigProvider = sharedData.EnemyWavesConfigProvider;
			_enemyConfigProvider = sharedData.EnemyConfigProvider;

			var allWaves = _enemyWavesConfigProvider.AllWaves;
			var totalEnemies = 0;

			var maxSpawnedQuantityPerConfig = new Dictionary<IEnemyConfig, int>();
			var maxSpawnedQuantityOfProjectiles = new Dictionary<IProjectileConfig, int>();

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

				if (!maxSpawnedQuantityPerConfig.ContainsKey(config))
				{
					maxSpawnedQuantityPerConfig.Add(config, 0);
				}

				maxSpawnedQuantityPerConfig[config] += waveConfig.MaxSpawnedQuantity;
				totalEnemies += waveConfig.MaxSpawnedQuantity;

				if (config.HasProperty<IShootProjectileAtPlayerEnemyPropertyConfig>())
				{
					var shootProjectileAtPlayerEnemyPropertyConfig = config.GetProperty<IShootProjectileAtPlayerEnemyPropertyConfig>();
					
					if (!maxSpawnedQuantityOfProjectiles.ContainsKey(shootProjectileAtPlayerEnemyPropertyConfig.ProjectileConfig))
					{
						maxSpawnedQuantityOfProjectiles.Add(shootProjectileAtPlayerEnemyPropertyConfig.ProjectileConfig, 0);
					}
					
					maxSpawnedQuantityOfProjectiles[shootProjectileAtPlayerEnemyPropertyConfig.ProjectileConfig] += waveConfig.MaxSpawnedQuantity;
				}
			}

			foreach (var kvp in maxSpawnedQuantityPerConfig)
			{
				_pool.Add(kvp.Key.Identity, kvp.Key.Prefab, kvp.Value);
			}
			
			foreach (var kvp in maxSpawnedQuantityOfProjectiles)
			{
				_pool.Add(kvp.Key.Identity, kvp.Key.PrefabView, kvp.Value * PROJECTILE_COUNT_PER_ENEMY);
			}

			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(totalEnemies);
			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(totalEnemies);
			
			_kdTreeStorage.KdTree.Build(new Vector3[totalEnemies]);

			_sharedData = sharedData;
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var gameFilter = world.Filter<GameComponent>().Inc<GameTimeComponent>().Inc<WavesDataComponent>().End();
			var timePool = world.GetPool<GameTimeComponent>();
			var deadEnemiesPool = world.GetPool<WavesDataComponent>();

			foreach (var gameEntityId in gameFilter)
			{
				ref var time = ref timePool.Get(gameEntityId);
				ref var wavesData = ref deadEnemiesPool.Get(gameEntityId);

				var timespan = TimeSpan.FromSeconds(time.Time);

				var currentWavesConfigs = _enemyWavesConfigProvider.Get(timespan.Ticks);
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
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}

		private void CreateEnemy(EcsWorld world, Vector2 playerPosition, IEnemyConfig enemyConfig, IEnemyWaveConfig enemyWaveConfig)
		{
			if (!_pool.TryGet<EnemyView>(enemyConfig.Identity, out var poolObject))
			{
				return;
			}
			
			var entity = world.NewEntity();
			var freeTreeIndex = FindFreeTreeIndex();

			if (_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.ContainsKey(entity.Id))
			{
				_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Remove(entity.Id);
			}

			var enemyView = poolObject.Object;
			var randomPosition = CreateRandomPosition(playerPosition);
			
			enemyView.transform.position = randomPosition;
			
			_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Add(entity.Id, freeTreeIndex);
			_kdTreeStorage.KdTreePositionIndexToEntityIdMap[freeTreeIndex] = entity.Id;
			_kdTreeStorage.KdTree.Points[freeTreeIndex] = enemyView.transform.position;

			enemyView.SetEntityId(entity.Id);

			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = enemyView.gameObject;

			AddEnemyComponent(entity, enemyConfig, enemyWaveConfig);
			AddMovementComponent(entity);
			AddLayerComponent(entity, enemyView);
			AddStickToLocationBlockComponent(entity);
			AddTransformComponent(entity, enemyView);
			AddPositionComponent(entity, randomPosition);

			var lerpTransformComponent = AddLerpTransformComponent(entity);

			AddHealthComponent(entity, enemyConfig);

			AddPoolObjectComponent(entity, poolObject);
			
			entity.AddStackTryApplyDamageComponent();
			entity.AddBlockApplyDamageTimerComponent();

			if (enemyConfig.HasProperty<IShootProjectileAtPlayerEnemyPropertyConfig>())
			{
				var shootProjectileAtPlayerEnemyPropertyConfig = enemyConfig.GetProperty<IShootProjectileAtPlayerEnemyPropertyConfig>();
				var playerEntities = world.Filter<PlayerComponent>().End();
				
				foreach (var playerEntityIdx in playerEntities)
				{
					entity.AddOrUpdateEnemyTargetComponent(playerEntityIdx);
					break;
				}
				
				entity.AddShootProjectileAtTargetComponent(
					shootProjectileAtPlayerEnemyPropertyConfig.ProjectileConfig,
					shootProjectileAtPlayerEnemyPropertyConfig.Layer,
					shootProjectileAtPlayerEnemyPropertyConfig.ShootRateInSeconds,
					shootProjectileAtPlayerEnemyPropertyConfig.MinDistanceToTargetToShoot,
					shootProjectileAtPlayerEnemyPropertyConfig.MaxDistanceToTargetToShoot);
			}
			
			if (enemyConfig.HasProperty<IMoveOnPlayerEnemyPropertyConfig>())
			{
				var playerEntities = world.Filter<PlayerComponent>().End();
				
				foreach (var playerEntityIdx in playerEntities)
				{
					entity.AddOrUpdateEnemyTargetComponent(playerEntityIdx);
					break;
				}
				
				var moveOnPlayerEnemyPropertyConfig = enemyConfig.GetProperty<IMoveOnPlayerEnemyPropertyConfig>();
				entity.AddSpeedComponent(moveOnPlayerEnemyPropertyConfig.MovementConfig);
			}
			
			if (enemyConfig.HasProperty<ICollisionEnemyPropertyConfig>())
			{
				var propertyConfig = enemyConfig.GetProperty<ICollisionEnemyPropertyConfig>();
				entity.AddCollisionComponent(propertyConfig.CollisionAreaConfig, propertyConfig.LayerMask);
				entity.AddHasCollidedComponent();
			}
			
			if (enemyConfig.HasProperty<IDamageOnCollisionEnemyPropertyConfig>())
			{
				var propertyConfig = enemyConfig.GetProperty<IDamageOnCollisionEnemyPropertyConfig>();
				entity.AddDamageOnCollisionComponent(propertyConfig.DamageProvider);
			}
			
			if (enemyConfig.HasProperty<IDestroyOnCollisionEnemyPropertyConfig>())
			{
				entity.AddDestroyOnCollisionComponent();
			}
		}
		private int FindFreeTreeIndex()
		{
			for (var i = 0; i < _kdTreeStorage.KdTreePositionIndexToEntityIdMap.Length; i++)
			{
				var entityId = _kdTreeStorage.KdTreePositionIndexToEntityIdMap[i];
				if (_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.ContainsKey(entityId))
				{
					continue;
				}

				return i;
			}
			
			return -1;
		}

		private void AddMovementComponent(EcsEntity entity)
		{
			entity.Add<MovementComponent>();
		}

		private static void AddEnemyComponent(EcsEntity entity, IEnemyConfig enemyConfig, IEnemyWaveConfig enemyWaveConfig)
		{
			ref var enemy = ref entity.Add<EnemyComponent>();
			enemy.ConfigId = enemyConfig.Id;
			enemy.WaveId = enemyWaveConfig.Id;
		}

		private void AddLayerComponent(EcsEntity entity, EnemyView enemyView)
		{
			ref var layer = ref entity.Add<LayerComponent>();
			layer.Layer = enemyView.gameObject.layer;
		}

		private void AddStickToLocationBlockComponent(EcsEntity entity)
		{
			entity.Add<StickToLocationBlockComponent>();
		}

		private static Vector2 CreateRandomPosition(Vector2 playerPosition)
		{
			var randomPosition = playerPosition + Random.insideUnitCircle*45f;
			var distanceToPlayer = Vector2.Distance(randomPosition, playerPosition);

			if (distanceToPlayer < 15f)
			{
				var direction = (randomPosition - playerPosition).normalized;
				randomPosition = playerPosition + direction*Random.Range(15f, 45f);
			}
			return randomPosition;
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

		private static void AddPoolObjectComponent(EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var poolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			poolObjectComponent.ViewPoolObject = viewPoolObject;
		}
	}
}