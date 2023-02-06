using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
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
			entity.AddMovementComponent();
			entity.AddRotationComponent(player.transform.right.ToVector2XY());
			entity.AddRigidbodyComponent(player.Rigidbody2D);
			entity.AddSkillsComponent();
			
			var healthComponent = entity.AddHealthComponent(spawnConfig.StartHealth, spawnConfig.StartHealth);
			entity.AddSpeedComponent(spawnConfig.MovementConfig);
			entity.AddGoldComponent();
			entity.AddProfileExperienceComponent();

			AddExperienceComponent(entity, sharedData.PlayersLevelsConfigProvider.Get(0), sharedData.GameModel.PlayerModel);
			
			UpdatePlayerModel(sharedData, healthComponent);

			sharedData.SetPlayerPositionProvider(player);
			
			CreateWeapon(entity.Id, sharedData);
		}

		private void CreateWeapon(int playerEntityId, ISharedData sharedData)
		{
			if (string.IsNullOrEmpty(sharedData.PlayerWeaponId))
			{
				return;
			}

			var config = sharedData.WeaponConfigProvider.Get(sharedData.PlayerWeaponId);

			var skillsPool = _world.GetPool<SkillsComponent>();
			ref var playerSkills = ref skillsPool.Get(playerEntityId);
			
			playerSkills.Levels.Add(config.SkillName, 1);
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

		private void AddExperienceComponent(EcsEntity entity, IPlayerLevelConfig levelConfig, IPlayerModel playerModel)
		{
			ref var exp = ref entity.AddExperienceComponent(0, levelConfig.ExperienceToNextLevel, 0);

			var data = playerModel.ToData();
			data.Experience = exp.Value;
			data.MaxExperience = exp.MaxValue;
			
			playerModel.Update(data);
		}

		private static void UpdatePlayerModel(ISharedData sharedData, HealthComponent healthComponent)
		{
			var playerModel = sharedData.GameModel.PlayerModel;
			var playerData = playerModel.ToData();
			playerData.Health = healthComponent.CurrentHealth;
			playerData.MaxHealth = healthComponent.MaxHealth;
			playerModel.Update(playerData);
		}
	}
}