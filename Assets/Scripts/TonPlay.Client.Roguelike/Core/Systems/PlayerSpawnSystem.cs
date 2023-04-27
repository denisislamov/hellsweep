using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Views;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerSpawnSystem : IEcsInitSystem
	{
		private EcsWorld _world;

		private readonly KdTreeStorage _kdTreeStorage;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;

		public PlayerSpawnSystem(
			KdTreeStorage kdTreeStorage,
			IMetaGameModelProvider metaGameModelProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider)
		{
			_kdTreeStorage = kdTreeStorage;
			_metaGameModelProvider = metaGameModelProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
		}

		public void Init(EcsSystems systems)
		{
			_world = systems.GetWorld();

			var sharedData = systems.GetShared<SharedData>();
			var spawnConfig = sharedData.PlayerConfigProvider.Get();

			var player = CreateViewAndEntity(spawnConfig, out var entity);

			CalculateUserStats(out var armor, out var health, out var damage);

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

			var startHealth = spawnConfig.StartHealth + health;
			var healthComponent = entity.AddHealthComponent(startHealth, startHealth);
			entity.AddSpeedComponent(spawnConfig.MovementConfig.StartSpeed);
			entity.AddGoldComponent();
			entity.AddProfileExperienceComponent();
			entity.AddStackTryApplyDamageComponent();
			entity.AddBlockApplyDamageTimerComponent();
			entity.Add<FlipSpriteInRotationDirectionComponent>();

			entity.AddBaseDamageComponent(damage);

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

		private void CalculateUserStats(out uint armor, out uint health, out uint damage)
		{
			armor = 0;
			health = 0;
			damage = 0;
			
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			foreach (var kvp in inventoryModel.Slots)
			{
				var slot = kvp.Value;

				if (slot.ItemId?.Value is null) continue;

				var itemModel = inventoryModel.GetItemModel(slot.ItemId.Value);

				if (itemModel is null) continue;

				var itemConfig = _inventoryItemsConfigProvider.Get(itemModel.ItemId.Value);
				var detailConfig = itemConfig.GetDetails(itemModel.DetailId.Value);

				switch (itemConfig.AttributeName)
				{
					case AttributeName.ATTACK:
						damage += detailConfig.Value;
						break;
					case AttributeName.HEALTH:
						health += detailConfig.Value;
						break;
					case AttributeName.ARMOR:
						armor += detailConfig.Value;
						break;
				}
			}
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