using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Views;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Player.Views.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerSpawnSystem : IEcsInitSystem
	{
		private EcsWorld _world;

		private readonly KdTreeStorage _kdTreeStorage;

		public PlayerSpawnSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_world = systems.GetWorld();

			var sharedData = systems.GetShared<SharedData>();
			var spawnConfig = sharedData.PlayerConfigProvider.Get();

			var player = CreateViewAndEntity(spawnConfig, out var entity);

			AddPlayerComponent(entity, spawnConfig);
			entity.AddLayerComponent(LayerMask.NameToLayer("Player"));
			entity.AddMovementComponent();
			entity.AddPositionComponent(player.Position);
			entity.AddRotationComponent(player.transform.right.ToVector2XY());
			entity.AddRigidbodyComponent(player.Rigidbody2D);
			entity.AddSkillsComponent();
			entity.AddCollisionComponent(
				CollisionAreaFactory.Create(spawnConfig.CollisionAreaConfig),
				spawnConfig.CollisionAreaMask);
			entity.AddCollisionAreaWithCollectablesComponent(
				CollisionAreaFactory.Create(spawnConfig.CollectablesCollisionAreaConfig));

			entity.AddAnimatorComponent(player.Animator);
			entity.AddBloodAnimatorComponent(player.BloodAnimator);
			entity.AddAnimationsComponent(player.AttackAnimationDuration);
			entity.AddSpriteRenderersComponent(player.SpriteRenderers);
			entity.AddDamageMultiplierComponent(1f);
			entity.AddSkillDurationMultiplierComponent(1f);
			
			var healthComponent = entity.AddHealthComponent(spawnConfig.StartHealth, spawnConfig.StartHealth);
			entity.AddSpeedComponent(spawnConfig.MovementConfig);
			entity.AddGoldComponent();
			entity.AddProfileExperienceComponent();
			entity.AddStackTryApplyDamageComponent();
			entity.AddBlockApplyDamageTimerComponent();
			entity.Add<FlipSpriteInRotationDirectionComponent>();

			AddExperienceComponent(entity, sharedData.PlayersLevelsConfigProvider.Get(0), sharedData.GameModel.PlayerModel);

			UpdatePlayerModel(sharedData, healthComponent);

			sharedData.SetPlayerPositionProvider(player);

			CreateWeapon(entity.Id, sharedData);

			_kdTreeStorage.CreateKdTreeIndexToEntityIdMap(1);
			_kdTreeStorage.CreateEntityIdToKdTreeIndexMap(1);

			_kdTreeStorage.KdTree.Build(new Vector2[] {player.Position});

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, player.Position);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);
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