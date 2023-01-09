using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class BasicEnemySpawnSystem : IEcsInitSystem
	{
		private const float SPEED = 1f;
		private const int ENEMIES_COUNT = 1000;

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();

			var spawnConfig = sharedData.EnemyConfigProvider.Get();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			for (var i = 0; i < ENEMIES_COUNT; i++)
			{
				var enemyView = CreateViewAndEntity(spawnConfig, world, out var entity);
				var randomPosition = CreateRandomPosition(playerPosition);

				enemyView.transform.position = randomPosition + Vector2.up;

				var entityIdProvider = AddEntityIdProvider(enemyView, entity.Id);

				ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
				viewProviderComponent.View = enemyView.gameObject;
				
				AddEnemyComponent(entity, spawnConfig);
				AddMovementComponent(entity);
				AddLayerComponent(entity, enemyView);
				AddCollidersComponent(entity, enemyView);
				AddTransformComponent(entity, enemyView);
				AddPositionComponent(entity, randomPosition);

				var lerpTransformComponent = AddLerpTransformComponent(entity);

				AddSpeedComponent(entity, lerpTransformComponent);
				AddHealthComponent(entity, spawnConfig);
				AddDamageOnCollisionComponent(entity, spawnConfig);
			}
		}

		private EntityIdProvider AddEntityIdProvider(EnemyView enemyView, int entityId)
		{
			var entityIdProvider = enemyView.gameObject.AddComponent<EntityIdProvider>();
			entityIdProvider.SetEntityId(entityId);
			return entityIdProvider;
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
		
		private static EnemyView CreateViewAndEntity(IEnemyConfig config, EcsWorld world, out EcsEntity entity)
		{
			var enemy = Object.Instantiate(config.Prefab);
			entity = world.NewEntity();
			enemy.name = $"{enemy.name} ({entity.Id})";
			return enemy;
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
		
		private static void AddSpeedComponent(EcsEntity entity, LerpTransformComponent lerpTransformComponent)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = SPEED/lerpTransformComponent.ValueToInterpolate;
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
	}
}