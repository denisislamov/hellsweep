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
		private const int ENEMIES_COUNT = 300;

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();

			var spawnConfig = sharedData.EnemySpawnConfigProvider.Get();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			for (var i = 0; i < ENEMIES_COUNT; i++)
			{
				var enemyView = CreateViewAndEntity(spawnConfig, world, out var entity);
				var randomPosition = CreateRandomPosition(playerPosition);

				enemyView.transform.position = randomPosition;
				
				AddEnemyComponent(entity);
				AddCollidersComponent(entity, enemyView);
				AddTransformComponent(entity, enemyView);
				AddPositionComponent(entity, randomPosition);

				var lerpTransformComponent = AddLerpTransformComponent(entity);

				AddSpeedComponent(entity, lerpTransformComponent);
				AddHealthComponent(entity, spawnConfig);
				AddDamageOnCollisionComponent(entity, spawnConfig);
				
				sharedData.AddColliderWithEntityToMap(enemyView.Collider2D, entity);
			}
		}
		
		private static void AddEnemyComponent(EcsEntity entity)
		{
			entity.Add<EnemyComponent>();
		}
		
		private static EnemyView CreateViewAndEntity(IEnemySpawnConfig spawnConfig, EcsWorld world, out EcsEntity entity)
		{
			var enemy = Object.Instantiate(spawnConfig.Prefab);
			entity = world.NewEntity();
			return enemy;
		}
		
		private static Vector2 CreateRandomPosition(Vector2 playerPosition)
		{
			var randomPosition = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
			var distanceToPlayer = Vector2.Distance(randomPosition, playerPosition);

			if (distanceToPlayer < 7f)
			{
				var direction = (randomPosition - playerPosition).normalized;
				randomPosition = playerPosition + Vector2.one*7f + direction*Random.Range(0, 7f);
			}
			return randomPosition;
		}
		
		private static void AddDamageOnCollisionComponent(EcsEntity entity, IEnemySpawnConfig spawnConfig)
		{
			ref var damageOnCollisionComponent = ref entity.Add<DamageOnCollisionComponent>();
			damageOnCollisionComponent.Damage = spawnConfig.DamageOnCollision;
		}
		
		private static void AddHealthComponent(EcsEntity entity, IEnemySpawnConfig spawnConfig)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = spawnConfig.StartHealth;
			healthComponent.MaxHealth = spawnConfig.StartHealth;
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