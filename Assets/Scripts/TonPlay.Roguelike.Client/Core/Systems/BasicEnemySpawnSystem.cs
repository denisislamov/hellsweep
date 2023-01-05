using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class BasicEnemySpawnSystem : IEcsInitSystem
	{
		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<SharedData>();

			var spawnConfig = sharedData.EnemySpawnConfigProvider.Get();
			var playerPosition = sharedData.PlayerPositionProvider.Position;

			for (var i = 0; i < 300; i++)
			{
				var enemy = Object.Instantiate(spawnConfig.Prefab);
				var randomPosition = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
				var distanceToPlayer = Vector2.Distance(randomPosition, playerPosition);

				if (distanceToPlayer < 7f)
				{
					var direction = (randomPosition - playerPosition).normalized;
					randomPosition = playerPosition + Vector2.one * 7f + direction * Random.Range(0, 7f);
				}
				
				enemy.Rigidbody2D.position = randomPosition;
				
				var entity = world.NewEntity();
				entity.Add<EnemyComponent>();
			
				ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
				rigidbodyComponent.Rigidbody = enemy.Rigidbody2D;
			
				ref var healthComponent = ref entity.Add<HealthComponent>();
				healthComponent.CurrentHealth = spawnConfig.StartHealth;
				healthComponent.MaxHealth = spawnConfig.StartHealth;

				ref var damageOnCollisionComponent = ref entity.Add<DamageOnCollisionComponent>();
				damageOnCollisionComponent.Damage = spawnConfig.DamageOnCollision;
			}
		}
	}
}