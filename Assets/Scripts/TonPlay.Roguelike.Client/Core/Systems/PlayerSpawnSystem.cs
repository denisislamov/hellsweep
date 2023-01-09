using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Player.Views.Interfaces;
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
			var spawnConfig = sharedData.PlayerConfigProvider.Get();
			
			var player = CreateViewAndEntity(spawnConfig, out var entity);
			
			AddPlayerComponent(entity, spawnConfig);
			AddMovementComponent(entity);
			AddRotationComponent(entity, player);
			AddRigidbodyComponent(entity, player);
			var healthComponent = AddHealthComponent(entity, spawnConfig);

			UpdatePlayerModel(sharedData, healthComponent);

			sharedData.SetPlayerPositionProvider(player);
			
			AddToSharedMapEntityWithColliders(player, sharedData, entity);

			CreateWeapon(entity.Id, sharedData, player);
		}
		
		private void AddRotationComponent(EcsEntity entity, PlayerView playerView)
		{
			ref var rotationComponent = ref entity.Add<RotationComponent>();
			rotationComponent.Direction = playerView.transform.right;
		}

		private void CreateWeapon(int playerEntityId, ISharedData sharedData, IHasWeaponSpawnRoot playerWeaponSpawnRoot)
		{
			if (string.IsNullOrEmpty(sharedData.PlayerWeaponId))
			{
				return;
			}

			var config = sharedData.WeaponConfigProvider.Get(sharedData.PlayerWeaponId);

			var view = Object.Instantiate(config.Prefab, playerWeaponSpawnRoot.WeaponSpawnRoot);
			view.transform.localPosition = Vector3.zero;

			var entity = _world.NewEntity();
			
			ref var component = ref entity.Add<WeaponComponent>();
			component.FireDelay = config.FireDelay;
			component.FireType = config.FireType;
			component.OwnerEntityId = playerEntityId;
			component.WeaponConfigId = config.Id;
		}

		private PlayerView CreateViewAndEntity(IPlayerConfig config, out EcsEntity entity)
		{
			var player = Object.Instantiate(config.Prefab);
			entity = _world.NewEntity();
			return player;
		}
		
		private static void AddPlayerComponent(EcsEntity entity, IPlayerConfig playerConfig)
		{
			ref var playerComponent = ref entity.Add<PlayerComponent>();
			playerComponent.ConfigId = playerConfig.Id;
		}
		
		private void AddMovementComponent(EcsEntity entity)
		{
			entity.Add<MovementComponent>();
		}
		
		private static void AddRigidbodyComponent(EcsEntity entity, PlayerView player)
		{
			ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
			rigidbodyComponent.Rigidbody = player.Rigidbody2D;
		}
		
		private static HealthComponent AddHealthComponent(EcsEntity entity, IPlayerConfig config)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = config.StartHealth;
			healthComponent.MaxHealth = config.StartHealth;
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
		}
	}
}