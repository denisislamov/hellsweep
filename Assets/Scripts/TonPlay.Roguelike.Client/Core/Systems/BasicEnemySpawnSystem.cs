using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class BasicEnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const float SPEED = 1f;
		private const int ENEMIES_COUNT = 750;
		
		private readonly KdTreeStorage _kdTreeStorage;
		
		private IEnemyConfig _spawnConfig;
		private ICompositeViewPool _pool;
		private EnemyConfigViewPoolIdentity _poolIdentity;

		public BasicEnemySpawnSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			_spawnConfig = sharedData.EnemyConfigProvider.Get();

			_pool = sharedData.CompositeViewPool;
			_poolIdentity = new EnemyConfigViewPoolIdentity(_spawnConfig);
			
			_pool.Add(_poolIdentity, _spawnConfig.Prefab, ENEMIES_COUNT);
			
			_kdTreeStorage.CreateEnemiesKdTreeIndexToEntityIdMap(ENEMIES_COUNT);
			_kdTreeStorage.CreateEnemiesEntityIdToKdTreeIndexMap(ENEMIES_COUNT);

			for (var i = 0; i < ENEMIES_COUNT; i++)
			{
				CreateEnemy(world, playerPosition);
			}
		}
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<EnemyComponent>().Exc<DeadComponent>().Exc<InactiveComponent>().End();
			var sharedData = systems.GetShared<SharedData>();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			for (int i = 0; i < ENEMIES_COUNT - filter.GetEntitiesCount(); i++)
			{
				CreateEnemy(world, playerPosition);
			}
		}
		
		private void CreateEnemy(EcsWorld world, Vector2 playerPosition)
		{
			var entity = world.NewEntity();

			if (!_pool.TryGet<EnemyView>(_poolIdentity, out var poolObject))
			{
				return;
			}

			var enemyView = poolObject.Object;
			var randomPosition = CreateRandomPosition(playerPosition);

			var gameObject = enemyView.gameObject;
			gameObject.name = string.Format("{0} (entity: {1})", _spawnConfig.Prefab.gameObject.name, entity.Id.ToString());
			
			enemyView.transform.position = randomPosition + Vector2.up;

			if (enemyView.EntityId != EcsEntity.DEFAULT_ID)
			{
				var treeIndex = _kdTreeStorage.KdTreeEntityIdToPositionIndexMap[enemyView.EntityId];
				
				_kdTreeStorage.KdTreePositionIndexToEntityIdMap[treeIndex] = entity.Id;
				
				_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Remove(enemyView.EntityId);
				_kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Add(entity.Id, treeIndex);
			}
			
			enemyView.SetEntityId(entity.Id);

			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = enemyView.gameObject;

			AddEnemyComponent(entity, _spawnConfig);
			AddMovementComponent(entity);
			AddLayerComponent(entity, enemyView);
			AddCollidersComponent(entity, enemyView);
			AddTransformComponent(entity, enemyView);
			AddPositionComponent(entity, randomPosition);

			var lerpTransformComponent = AddLerpTransformComponent(entity);

			AddSpeedComponent(entity);
			AddHealthComponent(entity, _spawnConfig);
			AddDamageOnCollisionComponent(entity, _spawnConfig);

			AddPoolObjectComponent(entity, poolObject);
		}

		private void AddMovementComponent(EcsEntity entity)
		{
			entity.Add<MovementComponent>();
		}

		private static void AddEnemyComponent(EcsEntity entity, IEnemyConfig enemyConfig)
		{
			ref var enemy = ref entity.Add<EnemyComponent>();
			enemy.ConfigId = enemyConfig.Id;
		}
		
		private void AddLayerComponent(EcsEntity entity, EnemyView enemyView)
		{
			ref var layer = ref entity.Add<LayerComponent>();
			layer.Layer = enemyView.gameObject.layer;
		}
		
		private static Vector2 CreateRandomPosition(Vector2 playerPosition)
		{
			var randomPosition = Random.insideUnitCircle * 45f;
			var distanceToPlayer = Vector2.Distance(randomPosition, playerPosition);

			if (distanceToPlayer < 15f)
			{
				var direction = (randomPosition - playerPosition).normalized;
				randomPosition = playerPosition + direction * Random.Range(15f, 45f);
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
		
		private static void AddSpeedComponent(EcsEntity entity)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = SPEED;
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