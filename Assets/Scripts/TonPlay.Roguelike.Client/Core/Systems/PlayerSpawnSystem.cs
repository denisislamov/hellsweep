using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerSpawnSystem : IEcsInitSystem, IEcsDestroySystem
	{
		private EcsWorld _world;
		private IDisposable _collisionListener;

		public void Init(EcsSystems systems)
		{
			_world = systems.GetWorld();
			
			var sharedData = systems.GetShared<SharedData>();
			var spawnConfig = sharedData.PlayerSpawnConfigProvider.Get();
			
			var player = Object.Instantiate(spawnConfig.Prefab);
			var entity = _world.NewEntity();
			
			entity.Add<PlayerComponent>();
			
			ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
			rigidbodyComponent.Rigidbody = player.Rigidbody2D;
			
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = spawnConfig.StartHealth;
			healthComponent.MaxHealth = spawnConfig.StartHealth;
			
			_collisionListener = player.Rigidbody2D.OnCollisionStay2DAsObservable().Subscribe(collision => OnCollisionStay(collision));
			
			sharedData.SetPlayerPositionProvider(player);
		}
		
		public void Destroy(EcsSystems systems)
		{
			_collisionListener?.Dispose();
		}

		private void OnCollisionStay(Collision2D collision)
		{
			if (collision.rigidbody == null)
			{
				return;
			}
			
			var entity = _world.NewEntity();
			entity.Add<EventComponent>();
			
			ref var collisionEventComponent = ref entity.Add<CollisionEventComponent>();
			collisionEventComponent.CollidedRigidbody = collision.rigidbody;
		}
	}
}