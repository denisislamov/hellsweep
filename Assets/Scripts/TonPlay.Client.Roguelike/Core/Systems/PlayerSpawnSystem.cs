using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Player.Views.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Skills;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems
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
			AddSkillsComponent(entity);
			//AddTransformComponent(entity, player);
			var healthComponent = AddHealthComponent(entity, spawnConfig);
			AddSpeedComponent(entity, spawnConfig.MovementConfig);
			
			AddGoldComponent(entity);
			AddExperienceComponent(entity, sharedData.PlayersLevelsConfigProvider.Get(0), sharedData.GameModel.PlayerModel);
			AddProfileExperienceComponent(entity);
			
			UpdatePlayerModel(sharedData, healthComponent);

			sharedData.SetPlayerPositionProvider(player);
			
			AddToSharedMapEntityWithColliders(player, sharedData, entity);

			CreateWeapon(entity.Id, sharedData, player);
		}
		
		private void AddSkillsComponent(EcsEntity entity)
		{
			ref var skillsComponent = ref entity.Add<SkillsComponent>();
			skillsComponent.Levels = new Dictionary<SkillName, int>();
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
			var projectileConfig = config.GetProjectileConfig();

			var view = Object.Instantiate(config.Prefab, playerWeaponSpawnRoot.WeaponSpawnRoot);
			view.transform.localPosition = Vector3.zero;

			var entity = _world.NewEntity();
			
			ref var component = ref entity.Add<WeaponComponent>();
			ref var transform = ref entity.Add<TransformComponent>();
			component.FireDelay = config.FireDelay;
			component.FireType = config.FireType;
			component.OwnerEntityId = playerEntityId;
			component.WeaponConfigId = config.Id;
			component.ProjectileIdentity = new ProjectileConfigViewPoolIdentity(projectileConfig);
			transform.Transform = view.transform;

			
			sharedData.CompositeViewPool.Add(component.ProjectileIdentity, projectileConfig.PrefabView, 128);
		}

		private PlayerView CreateViewAndEntity(IPlayerConfig config, out EcsEntity entity)
		{
			var player = Object.Instantiate(config.Prefab);
			entity = _world.NewEntity();
			return player;
		}
		
		private static void AddSpeedComponent(EcsEntity entity, IMovementConfig movementConfig)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = movementConfig.StartSpeed;
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
		
		private static void AddTransformComponent(EcsEntity entity, PlayerView player)
		{
			ref var rigidbodyComponent = ref entity.Add<TransformComponent>();
			rigidbodyComponent.Transform = player.transform;
		}
		
		private static HealthComponent AddHealthComponent(EcsEntity entity, IPlayerConfig config)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = config.StartHealth;
			healthComponent.MaxHealth = config.StartHealth;
			return healthComponent;
		}

		private void AddExperienceComponent(EcsEntity entity, IPlayerLevelConfig levelConfig, IPlayerModel playerModel)
		{
			ref var exp = ref entity.Add<ExperienceComponent>();
			exp.Value = 0;
			exp.MaxValue = levelConfig.ExperienceToNextLevel;
			exp.Level = 0;

			var data = playerModel.ToData();
			data.Experience = exp.Value;
			data.MaxExperience = exp.MaxValue;
			
			playerModel.Update(data);
		}
		
		private static void AddProfileExperienceComponent(EcsEntity entity)
		{
			entity.Add<ProfileExperienceComponent>();
		}
		
		private static void AddGoldComponent(EcsEntity entity)
		{
			entity.Add<GoldComponent>();
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