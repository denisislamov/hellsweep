using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerSpawnSystem : IEcsInitSystem
	{
		private EcsWorld _world;

		public void Init(EcsSystems systems)
		{
			_world = systems.GetWorld();
			
			var sharedData = systems.GetShared<SharedData>();
			var spawnConfig = sharedData.PlayerSpawnConfigProvider.Get();
			
			var player = CreateViewAndEntity(spawnConfig, out var entity);
			
			AddPlayerComponent(entity);
			AddRigidbodyComponent(entity, player);
			var healthComponent = AddHealthComponent(entity, spawnConfig);

			UpdatePlayerModel(sharedData, healthComponent);

			sharedData.SetPlayerPositionProvider(player);
			
			AddToSharedMapEntityWithColliders(player, sharedData, entity);
		}

		private PlayerView CreateViewAndEntity(IPlayerSpawnConfig spawnConfig, out EcsEntity entity)
		{
			var player = Object.Instantiate(spawnConfig.Prefab);
			entity = _world.NewEntity();
			return player;
		}
		
		private static void AddPlayerComponent(EcsEntity entity)
		{
			entity.Add<PlayerComponent>();
		}
		
		private static void AddRigidbodyComponent(EcsEntity entity, PlayerView player)
		{
			ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
			rigidbodyComponent.Rigidbody = player.Rigidbody2D;
		}
		
		private static HealthComponent AddHealthComponent(EcsEntity entity, IPlayerSpawnConfig spawnConfig)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = spawnConfig.StartHealth;
			healthComponent.MaxHealth = spawnConfig.StartHealth;
			return healthComponent;
		}
		
		private static void UpdatePlayerModel(ISharedData sharedData, HealthComponent healthComponent)
		{
			var playerModel = sharedData.GameModel.PlayerModel;
			var playerData = playerModel.ToData();
			playerData.Health = healthComponent.CurrentHealth;
			playerData.MaxHealth = healthComponent.MaxHealth;
			playerModel.Update(playerData);
		}
		
		private static void AddToSharedMapEntityWithColliders(PlayerView player, SharedData sharedData, EcsEntity entity)
		{
			var attachedColliders = new Collider2D[player.Rigidbody2D.attachedColliderCount];
			
			player.Rigidbody2D.GetAttachedColliders(attachedColliders);
			
			foreach (var attached in attachedColliders)
			{
				sharedData.AddColliderWithEntityToMap(attached, entity);
			}
		}
	}
}