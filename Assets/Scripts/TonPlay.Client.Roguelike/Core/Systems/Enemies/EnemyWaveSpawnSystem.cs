using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Waves;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class EnemyWaveSpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		public const int PREPARE_BEFORE_BOSS_SPAWN_SECONDS = 10;
		private const int PROJECTILE_COUNT_PER_ENEMY = 8;
		
		private ICompositeViewPool _pool;
		private ILevelEnemyWaveConfigProvider _enemyWavesConfigProvider;
		private IEnemyConfigProvider _enemyConfigProvider;
		private ISharedData _sharedData;
		
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
			var deathEffectConfigs = new HashSet<IDeathEffectConfig>();

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
					var property = config.GetProperty<IShootProjectileAtPlayerEnemyPropertyConfig>();

					if (!maxSpawnedQuantityOfProjectiles.ContainsKey(property.ProjectileConfig))
					{
						maxSpawnedQuantityOfProjectiles.Add(property.ProjectileConfig, 0);
					}

					maxSpawnedQuantityOfProjectiles[property.ProjectileConfig] += waveConfig.MaxSpawnedQuantity * property.PooledProjectileCount;
				}

				if (config.HasProperty<ICanSpawnProjectileEnemyPropertyConfig>())
				{
					var propertyConfig = config.GetProperty<ICanSpawnProjectileEnemyPropertyConfig>();

					if (!maxSpawnedQuantityOfProjectiles.ContainsKey(propertyConfig.ProjectileConfig))
					{
						maxSpawnedQuantityOfProjectiles.Add(propertyConfig.ProjectileConfig, 0);
					}

					maxSpawnedQuantityOfProjectiles[propertyConfig.ProjectileConfig] += propertyConfig.PooledCount;
				}
			}

			foreach (var kvp in maxSpawnedQuantityPerConfig)
			{
				var enemyConfig = kvp.Key;
				var quantity = kvp.Value;
				
				_pool.Add(enemyConfig.Identity, enemyConfig.Prefab, quantity);

				if (enemyConfig.DeathEffectConfig != null)
				{
					deathEffectConfigs.Add(enemyConfig.DeathEffectConfig);
				}
			}

			foreach (var kvp in maxSpawnedQuantityOfProjectiles)
			{
 				_pool.Add(kvp.Key.Identity, kvp.Key.PrefabView, kvp.Value*PROJECTILE_COUNT_PER_ENEMY);
			}

			_sharedData = sharedData;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var gameFilter = world.Filter<GameComponent>().Inc<GameTimeComponent>().Inc<WavesDataComponent>().End();
			var timePool = world.GetPool<GameTimeComponent>();
			var deadEnemiesPool = world.GetPool<WavesDataComponent>();

			foreach (var gameEntityId in gameFilter)
			{
				ref var time = ref timePool.Get(gameEntityId);
				ref var wavesData = ref deadEnemiesPool.Get(gameEntityId);

				var timespan = TimeSpan.FromSeconds(time.Time);

				var currentWaveGroupConfig = _enemyWavesConfigProvider.Get(timespan.Ticks);

				if (currentWaveGroupConfig is null)
				{
					break;
				}

				var nextWaveGroupConfig = currentWaveGroupConfig.Next();
				var currentWavesConfigs = currentWaveGroupConfig.Waves;

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

					if (timespan.Ticks < waveConfig.StartTimingTicks)
					{
						continue;
					}

					wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id] -= TimeSpan.FromSeconds(Time.deltaTime).Ticks;

					var timeLeft = wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id];
					if (timeLeft > 0)
					{
						continue;
					}
					
					var currentWaveDuration = nextWaveGroupConfig is null
						? TimeSpan.FromTicks(-1)
						: TimeSpan.FromTicks(nextWaveGroupConfig.StartTimingTicks - waveConfig.StartTimingTicks);

					var leftEnemiesAmount = waveConfig.EnemiesQuantity - wavesData.WavesEnemiesKilledAmount[waveConfig.Id];
					var maxEnemiesSpawnedQuantity = Math.Min(leftEnemiesAmount, waveConfig.MaxSpawnedQuantity);
					var spawnQuantity = maxEnemiesSpawnedQuantity - wavesData.WavesEnemiesSpawnedAmount[waveConfig.Id];
					var spawnQuantityPerRate = (int) Math.Round(
						waveConfig.EnemiesQuantity
						/ currentWaveDuration.TotalSeconds
						* TimeSpan.FromTicks(waveConfig.SpawnTickRate).TotalSeconds);
					
					if (currentWaveDuration.Ticks < 0)
					{
						spawnQuantityPerRate = waveConfig.EnemiesQuantity / 60;
					}
					
					spawnQuantityPerRate = spawnQuantityPerRate <= 0 ? 1 : spawnQuantityPerRate;

					spawnQuantity = (int)Mathf.Clamp(spawnQuantity, 0, spawnQuantityPerRate);

					var spawnPosition = CreateRandomPosition(_sharedData.PlayerPositionProvider.Position);

					for (var i = 0; i < spawnQuantity; i++)
					{
						var enemyConfig = _enemyConfigProvider.Get(waveConfig.EnemyId);

						CreateEnemy(
							world,
							_sharedData.PlayerPositionProvider.Position,
							enemyConfig,
							waveConfig,
							spawnPosition,
							spawnQuantity, 
							waveConfig.StartHealth);

						wavesData.WavesEnemiesSpawnedAmount[waveConfig.Id]++;
					}

					wavesData.WavesTimeLeftToNextSpawn[waveConfig.Id] = waveConfig.SpawnTickRate;
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void CreateEnemy(EcsWorld world,
								 Vector2 playerPosition,
								 IEnemyConfig enemyConfig,
								 IEnemyWaveConfig enemyWaveConfig,
								 Vector2 randomizedWavePosition,
								 int groupSize, 
								 float health)
		{
			if (!_pool.TryGet<EnemyView>(enemyConfig.Identity, out var poolObject))
			{
				return;
			}

			var entity = world.NewEntity();


			var enemyView = poolObject.Object;
			var spawnPosition = GetSpawnPosition(playerPosition, enemyConfig.EnemyType, enemyWaveConfig.WaveSpawnType, randomizedWavePosition, groupSize);

			enemyView.transform.position = spawnPosition;

			enemyView.SetEntityId(entity.Id);

			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = enemyView.gameObject;

			AddEnemyComponent(entity, enemyConfig, enemyWaveConfig);
			AddMovementComponent(entity);
			AddLayerComponent(entity, enemyView);
			AddStickToLocationBlockComponent(entity);
			AddPositionComponent(entity, spawnPosition);
			AddTypedEnemyComponent(entity, enemyConfig.EnemyType);

			entity.AddTransformComponent(enemyView.transform);

			if (enemyView.Rigidbody2D != null)
			{
				entity.AddRigidbodyComponent(enemyView.Rigidbody2D);
			}

			if (enemyView.SpriteRenderers != null)
			{
				entity.AddSpriteRenderersComponent(enemyView.SpriteRenderers);
			}

			if (enemyView.Animator != null)
			{
				entity.AddAnimatorComponent(enemyView.Animator);
			}
			
			if (enemyView.PlayableDirector != null)
			{
				enemyView.PlayableDirector.Stop();
				enemyView.PlayableDirector.Play();
				
				entity.AddPlayableDirectorComponent(enemyView.PlayableDirector);
			}

			entity.Add<FlipSpriteInRotationDirectionComponent>();
			entity.Add<IgnoreTransformRotation>();
			entity.AddRotationComponent(Vector2.zero);
			entity.AddSyncRotationWithMovementDirectionComponent();

			AddHealthComponent(entity, health);

			AddPoolObjectComponent(entity, poolObject);

			entity.AddStackTryApplyDamageComponent();
			entity.AddBlockApplyDamageTimerComponent();
			entity.AddShowAppliedDamageIndicatorComponent();

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
					shootProjectileAtPlayerEnemyPropertyConfig.Quantity,
					shootProjectileAtPlayerEnemyPropertyConfig.FieldOfView,
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
				entity.AddSpeedComponent(moveOnPlayerEnemyPropertyConfig.MovementConfig.StartSpeed);
				entity.AddMoveToTargetComponent();
			}

			if (enemyConfig.HasProperty<ICollisionEnemyPropertyConfig>())
			{
				var propertyConfig = enemyConfig.GetProperty<ICollisionEnemyPropertyConfig>();
				entity.AddCollisionComponent(CollisionAreaFactory.Create(propertyConfig.CollisionAreaConfig), propertyConfig.LayerMask);
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

			if (enemyConfig.HasProperty<IPerformActionsOnSpawnEnemyPropertyConfig>())
			{
				var propertyConfig = enemyConfig.GetProperty<IPerformActionsOnSpawnEnemyPropertyConfig>();

				for (var i = 0; i < propertyConfig.Actions.Length; i++)
				{
					var action = propertyConfig.Actions[i];
					action.Execute(entity.Id, _sharedData);
				}
			}

			if (enemyConfig.HasProperty<IRotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig>())
			{
				var propertyConfig = enemyConfig.GetProperty<IRotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig>();
				var targetPosition = world.GetPool<PositionComponent>().Get(entity.Get<TargetComponent>().EntityId);
				var position = spawnPosition.ToVector2XY();
				var direction = targetPosition.Position - position;
				direction.Normalize();
				entity.AddRotateMovementInTargetDirectionWhenDistanceExceededComponent(propertyConfig.Distance, direction);
			}

			if (enemyConfig.HasProperty<ILookAtTargetEnemyPropertyConfig>())
			{
				entity.AddLookAtTargetComponent();
			}
		}

		private Vector3 GetSpawnPosition(Vector2 playerPosition, EnemyType type, WaveSpawnType waveSpawnType, Vector2 randomizedWaveSpawnPosition, int groupSize)
		{
			switch (waveSpawnType)
			{
				case WaveSpawnType.Group:
				{
					var minRandomSize = 3f;
					var dynamicRandomSize = groupSize/125f;
					var randomSize = dynamicRandomSize > minRandomSize ? dynamicRandomSize : minRandomSize;
					return randomizedWaveSpawnPosition + Random.insideUnitCircle*randomSize;
				}
			}

			switch (type)
			{
				case EnemyType.Boss:
					return playerPosition + Vector2.up*4;
				default:
					return CreateRandomPosition(playerPosition);
			}
		}

		private void AddTypedEnemyComponent(EcsEntity entity, EnemyType type)
		{
			switch (type)
			{
				case EnemyType.Boss:
					entity.Add<BossEnemy>();
					return;
				case EnemyType.Miniboss:
					entity.Add<MiniBossEnemy>();
					return;
				default:
					entity.Add<RegularEnemy>();
					return;
			}
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
				var direction = (randomPosition - playerPosition);
				direction.Normalize();
				randomPosition = playerPosition + direction*Random.Range(15f, 45f);
			}
			return randomPosition;
		}

		private static void AddHealthComponent(EcsEntity entity, float health)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = health;
			healthComponent.MaxHealth = health;
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

		private static void AddPoolObjectComponent(EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var poolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			poolObjectComponent.ViewPoolObject = viewPoolObject;
		}
	}
}