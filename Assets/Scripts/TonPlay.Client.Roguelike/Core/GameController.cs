using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Models;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Systems;
using TonPlay.Client.Roguelike.Core.Systems.Enemies;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.BossWorm;
using TonPlay.Client.Roguelike.Core.Systems.Skills;
using TonPlay.Client.Roguelike.Core.Systems.Skills.Active;
using TonPlay.Client.Roguelike.Core.Systems.Skills.Passive;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Core
{
	internal class GameController : MonoBehaviour, IInitializable, IGameController
	{
		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private Transform _blocksRoot;

		[SerializeField]
		private List<GameControllerExtension> _extensions;

		public ISharedData SharedData => _sharedData;
		public EcsWorld MainWorld => _mainWorld;
		public EcsWorld EffectsWorld => _effectsWorld;

		private EcsWorld _mainWorld;
		private EcsWorld _effectsWorld;

		private EcsSystems _updateSystems;
		private EcsSystems _collectablesSystem;
		private EcsSystems _changeHealthSystem;
		private EcsSystems _fixedUpdateSystems;
		private EcsSystems _animationSystems;
		private EcsSystems _spawnSystems;
		private EcsSystems _destroySystems;
		private EcsSystems _skillsSystems;
		private EcsSystems _syncKdTreePositionSystems;
		private EcsSystems _bossWormSystems;
		private EcsSystems _bossButcherSystems;

		private IDisposable _updateCycle;
		private IDisposable _fixedUpdateCycle;
		private IDisposable _initTimer;

		private IUIService _uiService;
		private IGameModel _gameModel;
		private SharedData _sharedData;

		private KdTreeStorage _enemyKdTreeStorage;
		private KdTreeStorage _playersKdTreeStorage;
		private KdTreeStorage _collectablesKdTreeStorage;
		private KdTreeStorage _playerProjectilesKdTreeStorage;
		private KdTreeStorage _arenasKdTreeStorage;
		private KdTreeStorage _locationBlocksKdTreeStorage;

		private OverlapExecutor _overlapExecutor;
		private ICollectableEntityFactory _collectablesEntityFactory;
		private ILocationConfigStorage _locationConfigStorage;

		private bool _inited;
		private KdTreeStorage[] _storages;
		private IMetaGameModelProvider _metaGameModelProvider;
		private IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private Vector2 _currentLocationSize;

		[Inject]
		public void Construct(
			IGameModelProvider gameModelProvider,
			IGameModelSetter gameModelSetter,
			IUIService uiService,
			SharedDataProvider sharedDataProvider,
			OverlapExecutor.Factory overlapExecutorFactory,
			SharedData.Factory sharedDataFactory,
			ILocationConfigStorage locationConfigStorage,
			CollectablesEntityFactory.Factory collectablesEntityFactoryFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider)
		{
			_uiService = uiService;
			_locationConfigStorage = locationConfigStorage;
			_metaGameModelProvider = metaGameModelProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;

			CreateGameModel(gameModelSetter);

			_mainWorld = new EcsWorld(new EcsWorld.Config()
			{
				Entities = 2048
			});
			
			_effectsWorld = new EcsWorld(new EcsWorld.Config()
			{
				Entities = 1024
			});

			_enemyKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("Enemy"));
			_playersKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("Player"));
			_collectablesKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("Utility"));
			_playerProjectilesKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("PlayerProjectile"));
			_arenasKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("Arena"));
			_locationBlocksKdTreeStorage = new KdTreeStorage(LayerMask.NameToLayer("LocationBlock"));

			_storages = new KdTreeStorage[]
			{
				_enemyKdTreeStorage,
				_playersKdTreeStorage,
				_collectablesKdTreeStorage,
				_playerProjectilesKdTreeStorage,
				_arenasKdTreeStorage,
				_locationBlocksKdTreeStorage
			};

			_playerProjectilesKdTreeStorage.CreateEntityIdToKdTreeIndexMap(RoguelikeConstants.Core.PLAYER_PROJECTILES_MAX_COUNT);
			_playerProjectilesKdTreeStorage.CreateKdTreeIndexToEntityIdMap(RoguelikeConstants.Core.PLAYER_PROJECTILES_MAX_COUNT);
			_playerProjectilesKdTreeStorage.KdTree.Build(new Vector2[RoguelikeConstants.Core.PLAYER_PROJECTILES_MAX_COUNT]);
			
			_arenasKdTreeStorage.CreateEntityIdToKdTreeIndexMap(RoguelikeConstants.Core.ARENA_MAX_COUNT);
			_arenasKdTreeStorage.CreateKdTreeIndexToEntityIdMap(RoguelikeConstants.Core.ARENA_MAX_COUNT);
			_arenasKdTreeStorage.KdTree.Build(new Vector2[RoguelikeConstants.Core.ARENA_MAX_COUNT]);
			
			_enemyKdTreeStorage.CreateEntityIdToKdTreeIndexMap(RoguelikeConstants.Core.KD_TREE_ENEMY_INIT_COUNT);
			_enemyKdTreeStorage.CreateKdTreeIndexToEntityIdMap(RoguelikeConstants.Core.KD_TREE_ENEMY_INIT_COUNT);
			_enemyKdTreeStorage.KdTree.Build(new Vector2[RoguelikeConstants.Core.KD_TREE_ENEMY_INIT_COUNT]);

			_sharedData = sharedDataFactory.Create();
			_overlapExecutor = overlapExecutorFactory.Create(_mainWorld, _storages);

			var weaponItemId = GetPlayerWeaponItemId(metaGameModelProvider);
			_sharedData.SetPlayerWeapon(weaponItemId);
			_sharedData.SetCollectablesKdTreeStorage(_collectablesKdTreeStorage);
			_sharedData.SetArenasKdTreeStorage(_arenasKdTreeStorage);
			_sharedData.SetMainWorld(_mainWorld);
			_sharedData.SetEffectsWorld(_effectsWorld);

			_collectablesEntityFactory = collectablesEntityFactoryFactory.Create(_sharedData);
			
			sharedDataProvider.SetSharedData(_sharedData);

			Initialize();
		}

		public void Initialize()
		{
			AddCameraToEcsWorld();

			_spawnSystems = new EcsSystems(_mainWorld, _sharedData)
						   .AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
						   .Add(new PoolObjectCreateSystem())
						   .Add(new PlayerSpawnSystem(_playersKdTreeStorage, _metaGameModelProvider, _inventoryItemsConfigProvider))
						   .Add(new EnemyWaveSpawnSystem())
						   .Add(new CollectablesSpawnSystem(_collectablesKdTreeStorage))
						   .Add(new CollectablesSpawnOnEnemyDiedEventSystem(_collectablesEntityFactory))
						   .Add(new GoldCollectablesSpawnSystem(_collectablesEntityFactory))
						   .Add(new HealthCollectablesSpawnSystem(_collectablesEntityFactory))
						   .Add(new MagnetCollectablesSpawnSystem(_collectablesEntityFactory))
						   .Add(new BombCollectablesSpawnSystem(_collectablesEntityFactory))
						   .Add(new LocationSpawnSystem(_blocksRoot, _locationBlocksKdTreeStorage, _locationConfigStorage))
						   .Add(new SpawnExperienceCollectablesToGainFirstLevelsSystem(_collectablesEntityFactory));

			_updateSystems = new EcsSystems(_mainWorld, _sharedData)
							.AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
							.Add(new GameSystem())
							.Add(new EvaluatePlayableDirector())
							.Add(new EnemiesKdTreeSystem(_enemyKdTreeStorage))
							.Add(new ActiveMagnetSystem(_collectablesKdTreeStorage))
							.Add(new PrepareToExplodeCollectedBombsSystem())
							.Add(new StickEaseMovementToEntityPositionSystem())
							.Add(new EaseMovementSystem())
							.Add(new SpinAroundEntityPositionMovementSystem())
							.Add(new RotationLookAtTargetSystem())
							.Add(new InvertRegularEnemyRotationLookAtTargetOnBossAppearingSystem())
							.Add(new SyncRotationWithPositionDifferenceSystem())
							.Add(new SyncRotationWithMovementDirectionSystem())
							.Add(new SyncPositionWithAnotherEntitySystem())
							.Add(new SyncRotationWithAnotherEntitySystem())
							.Add(new DamageOnDistanceChangeSystem(_overlapExecutor))
							.Add(new TransformPositionSystem())
							.Add(new SmoothCameraMovementFollowSystem())
							.Add(new CameraShakeAndFollowSystem())
							.Add(new EnemyMovementToTargetSystem())
							.Add(new EnemyRotateMovementDirectionToTargetWhenDistanceExceededSystem())
							.Add(new RegularEnemyMoveOutOnBossSpawnSystem())
							.Add(new EnemyMovementAroundEnemiesSystem(_overlapExecutor))
							.Add(new EnemyShootAtTargetSystem())
							.Add(new PlayerCollisionWithCollectablesSystem(_overlapExecutor))
							.Add(new FadeColorAppliedDamageIndicatorSystem())
							.Add(new RoninOyoroiSkillSystem()) // - we might refactor it later, now it's here to reduce applied damage
							.Add(new ApplyDamageSystem())
							.Add(new PlayerLevelUpgradeSystem(_uiService))
							.Add(new AccelerationSystem())
							.Add(new InvertMovementAxisOnSpeedInversionSystem())
							.Add(new LocalSpaceTransformMovementSystem())
							.Add(new GlobalSpaceTransformMovementSystem())
							.Add(new ReduceApplyForceSystem())
							.Add(new TransformRotationSystem())
							.Add(new UpdatePlayerModelSystem())
							.Add(new UpdateBossModelSystem())
							.Add(new CollisionSystem(_overlapExecutor))
							.Add(new DamageOnCollisionSystem())
							.Add(new ProjectileExplodeOnMoveDistanceSystem())
							.Add(new ProjectileExplodeOnCollisionSystem())
							.Add(new ProjectileExplodeOnTimerSystem())
							.Add(new ExplosionSystem(_overlapExecutor))
							.Add(new BossShooterRicochetProjectileOffTheArenaSystem())
							.Add(new BlockTimerApplyDamageSystem())
							.Add(new TryApplyDamageSystem())
							.Add(new ApplyCameraShakeSystem())
							.Add(new ShowAppliedDamageSystem())
							.Add(new SpawnAppliedDamageIndicatorSystem())
							.Add(new DrawDebugKdTreePositionSystem())
							.Add(new LocationMoveSystem(_locationBlocksKdTreeStorage, _locationConfigStorage))
							.Add(new GameOverSystem(_uiService))
							.Add(new VictorySystem(_uiService))
				;

			_bossWormSystems = new EcsSystems(_mainWorld, _sharedData)
			   .Add(new BossWormFollowStateSystem())
			   .Add(new BossWormTankStateSystem())
			   .Add(new BossWormShootStateSystem());

			_bossButcherSystems = new EcsSystems(_mainWorld, _sharedData)
								 .Add(new BossButcherFollowStateSystem())
								 .Add(new BossButcherTankStateSystem());

			_syncKdTreePositionSystems = new EcsSystems(_mainWorld, _sharedData)
										.Add(new UpdateKdTreeElementPositionSystem())
										.Add(new RebuildKdTreeSystem(_storages));

			_skillsSystems = new EcsSystems(_mainWorld, _sharedData)
							.AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
							.Add(new RPGSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new GuardianSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new BrickSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new DrillShotSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new HolyWaterSkillSystem())
							.Add(new ForcefieldDeviceSkillSystem(_overlapExecutor, _playerProjectilesKdTreeStorage))
							.Add(new RevolverSkillSystem(_overlapExecutor, _playerProjectilesKdTreeStorage))
							.Add(new ShurikenSkillSystem(_overlapExecutor, _playerProjectilesKdTreeStorage))
							.Add(new CrossbowSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new KatanaSkillSystem(_playerProjectilesKdTreeStorage))
							.Add(new FitnessGuideSkillSystem())
							.Add(new EnergyDrinkSkillSystem())
							.Add(new SportShoesSkillSystem())
							.Add(new HiPowerBulletSkillSystem())
							.Add(new HiPowerMagnetSkillSystem())
							.Add(new ExoBracerSkillSystem())
							 ;

			_animationSystems = new EcsSystems(_mainWorld, _sharedData)
							   .AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
							   .Add(new AttackAnimationAnimatorSystem())
							   .Add(new RunAnimationAnimatorSystem())
							   .Add(new RunBloodAnimationAnimatorSystem())
							   .Add(new FlipSpriteInRotationDirectionSystem());

			_fixedUpdateSystems = new EcsSystems(_mainWorld, _sharedData)
								 .AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
								 .Add(new PlayerMovementInputSystem())
								 .Add(new RigidbodyMovementSystem())
								 .Add(new RigidbodyPositionSystem());
			
			_collectablesSystem = new EcsSystems(_mainWorld, _sharedData)
								 .Add(new ApplyCollectablesSystem())
								 .Add(new ApplyProfileExperienceCollectableSystem())
								 .Add(new ApplyExperienceCollectableSystem())
								 .Add(new ApplyGoldCollectableSystem())
								 .Add(new ApplyHealthCollectableSystem())
								 .Add(new ApplyMagnetCollectableSystem())
								 .Add(new ApplyBombCollectableSystem());
			
			_changeHealthSystem = new EcsSystems(_mainWorld, _sharedData)
			   .Add(new ChangeHealthSystem());

			_destroySystems = new EcsSystems(_mainWorld, _sharedData)
							 .AddWorld(_effectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
							 .Add(new UpdateWaveDataOnEnemyDeathSystem())
							 .Add(new PerformActionsOnEnemyDeathSystem())
							 .Add(new SpawnEffectOnDestroySystem())
							 .Add(new SpawnEnemyDeathEffectSystem())
							 .Add(new SyncDestroyEntitySystem())
							 .Add(new DestroyOnReceiveDamageSystem())
							 .Add(new DestroyIfDistanceExceededSystem())
							 .Add(new DestroyOnTimerSystem())
							 .Add(new DestroyOnCollisionSystem())
							 .Add(new DestroyPoolObjectSystem())
							 .Add(new DestroyNonPoolGameObjectSystem())
							 .Add(new ClearUsedEventsSystem())
							 .Add(new ClearAttackEventsSystem())
							 .Add(new ClearHasCollidedComponentsSystem())
							 .Add(new ClearDeadEntityDataSystem())
							 .Add(new SpawnProjectileOnDestroySystem())
							 .Add(new ClearDestroyedOrDeadElementsFromKdTreeSystem())
							 .Add(new DestroyEntitySystem());

			_spawnSystems.Init();
			_collectablesSystem.Init();
			_changeHealthSystem.Init();
			_updateSystems.Init();
			_bossWormSystems.Init();
			_bossButcherSystems.Init();
			_syncKdTreePositionSystems.Init();
			_skillsSystems.Init();
			_animationSystems.Init();
			_fixedUpdateSystems.Init();
			_destroySystems.Init();
			
			for (var i = 0; i < _extensions.Count; i++)
			{
				_extensions[i].Setup(this);
				_extensions[i].OnInit();
			}

			_uiService.Open<GameScreen, IGameScreenContext>(new GameScreenContext());

			FinishInit();
		}

		private void FinishInit()
		{
			_inited = true;
		}

		private void Update()
		{
			if (!_inited || _gameModel.Paused.Value) return;

			_spawnSystems?.Run();
			_updateSystems?.Run();
			_bossWormSystems?.Run();
			_bossButcherSystems?.Run();
			_syncKdTreePositionSystems?.Run();
			_skillsSystems?.Run();
			_animationSystems?.Run();
			_collectablesSystem?.Run();
			_changeHealthSystem?.Run();
			_destroySystems?.Run();
			
			for (var i = 0; i < _extensions.Count; i++)
			{
				_extensions[i].OnUpdate();
			}
		}

		private void FixedUpdate()
		{
			if (!_inited || _gameModel.Paused.Value) return;

			_fixedUpdateSystems?.Run();
			
			for (var i = 0; i < _extensions.Count; i++)
			{
				_extensions[i].OnFixedUpdate();
			}
		}

		private void OnDestroy()
		{
			_updateCycle?.Dispose();
			_fixedUpdateCycle?.Dispose();

			_spawnSystems?.Destroy();
			_updateSystems?.Destroy();
			_bossWormSystems?.Destroy();
			_bossButcherSystems?.Destroy();
			_syncKdTreePositionSystems?.Destroy();
			_skillsSystems?.Destroy();
			_animationSystems?.Destroy();
			_collectablesSystem?.Destroy();
			_changeHealthSystem?.Destroy();
			_destroySystems?.Destroy();
		}

		private void CreateGameModel(IGameModelSetter gameModelSetter)
		{
			_gameModel = new GameModel();
			gameModelSetter.Set(_gameModel);
		}

		private void AddCameraToEcsWorld()
		{
			var cameraEntityId = _mainWorld.NewEntity();
			cameraEntityId.Add<CameraComponent>();
			cameraEntityId.Add<CameraShakeComponent>();
			ref var cameraTransformComponent = ref cameraEntityId.Add<TransformComponent>();
			cameraTransformComponent.Transform = _camera.transform;
		}
		
		private string GetPlayerWeaponItemId(IMetaGameModelProvider metaGameModelProvider)
		{
			var inventoryModel = metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var userItemId = inventoryModel.Slots[SlotName.WEAPON]?.ItemId?.Value;
			var weaponItemId = inventoryModel.GetItemModel(userItemId)?.ItemId?.Value;
			//todo hardcoded katana
			return weaponItemId ?? "bae7a647-359a-4bb5-ae6b-7181a616cf7f";
		}
	}
}