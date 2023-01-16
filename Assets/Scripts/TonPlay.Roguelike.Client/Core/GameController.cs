using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Models;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Systems;
using TonPlay.Roguelike.Client.Core.Systems.Skills;
using TonPlay.Roguelike.Client.UI.Screens.Game;
using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Core
{
	public class GameController : MonoBehaviour, IInitializable
	{
		[SerializeField]
		private Camera _camera;

		private EcsWorld _world;
		private EcsSystems _updateSystems;
		private EcsSystems _collectablesSystem;
		private EcsSystems _fixedUpdateSystems;
		private EcsSystems _spawnSystems;
		private EcsSystems _destroySystems;
		private EcsSystems _skillsSystems;

		private IDisposable _updateCycle;
		private IDisposable _fixedUpdateCycle;
		private IDisposable _initTimer;

		private IUIService _uiService;
		private IGameModelProvider _gameModelProvider;
		private IGameModel _gameModel;
		private SharedData _sharedData;

		private KdTreeStorage _enemyKdTreeStorage;
		private KdTreeStorage _collectablesKdTreeStorage;

		private OverlapExecutor _overlapExecutor;

		private bool _inited;

		[Inject]
		public void Construct(
			IGameModelProvider gameModelProvider,
			IGameModelSetter gameModelSetter,
			IUIService uiService,
			OverlapExecutor.Factory overlapExecutorFactory,
			SharedData.Factory sharedDataFactory)
		{
			_uiService = uiService;

			CreateGameModel(gameModelProvider, gameModelSetter);

			_world = new EcsWorld();

			_enemyKdTreeStorage = new KdTreeStorage();
			_collectablesKdTreeStorage = new KdTreeStorage();

			_sharedData = sharedDataFactory.Create();
			_overlapExecutor = overlapExecutorFactory.Create(
				_world,
				new KdTreeStorage[] {_enemyKdTreeStorage, _collectablesKdTreeStorage});

			_sharedData.SetPlayerWeapon("bow");

			Initialize();
		}

		public void Initialize()
		{
			AddCameraToEcsWorld();

			_spawnSystems = new EcsSystems(_world, _sharedData)
						   .Add(new PlayerSpawnSystem())
						   .Add(new BasicEnemySpawnSystem(_enemyKdTreeStorage))
						   .Add(new CollectablesSpawnSystem(_collectablesKdTreeStorage))
						   .Add(new CollectablesSpawnOnEnemyDiedEventSystem(_collectablesKdTreeStorage));

			var kdTreesSystem = new KdTreesSystem(_enemyKdTreeStorage);

			_updateSystems = new EcsSystems(_world, _sharedData)
							.Add(new GameSystem())
							.Add(new WeaponFireSystem())
							.Add(new WeaponFireBlockSystem())
							.Add(new WeaponFireBlockSystem())
							.Add(new WeaponRotationSystem())
							.Add(new WeaponPositionSystem())
							.Add(new TransformPositionSystem())
							.Add(new CameraMovementSystem())
							.Add(new BasicEnemyMovementTargetSystem(_overlapExecutor))
							.Add(new PlayerCollisionSystem(_overlapExecutor))
							.Add(new ApplyDamageSystem())
							.Add(new PlayerLevelUpgradeSystem(_uiService))
							.Add(new AccelerationSystem())
							.Add(new TransformMovementSystem())
							.Add(new UpdatePlayerModelSystem())
							.Add(new ProjectileCollisionSystem(_overlapExecutor))
							.Add(new ProjectileExplodeOnMoveDistanceSystem(_overlapExecutor))
							.Add(kdTreesSystem)
							.Add(new GameOverSystem());

			_skillsSystems = new EcsSystems(_world, _sharedData)
						    .Add(new RPGSkillSystem());

			_fixedUpdateSystems = new EcsSystems(_world, _sharedData)
								 .Add(new PlayerMovementInputSystem())
								 .Add(new RigidbodyMovementSystem())
								 .Add(new RigidbodyPositionSystem());

			_destroySystems = new EcsSystems(_world, _sharedData)
							 .Add(new DestroyOnTimerSystem())
							 .Add(new DestroyOnCollisionSystem())
							 .Add(new DestroyPoolObjectSystem())
							 .Add(new ClearUsedEventsSystem())
							 .Add(new ClearHasCollidedComponentsSystem())
							 .Add(new ClearDeadEnemiesFromKdTreeSystem(_enemyKdTreeStorage))
							 .Add(new ClearDestroyedCollectablesFromKdTreeSystem(_collectablesKdTreeStorage))
							 .Add(new ClearDeadEntityDataSystem())
							 .Add(new DestroyEntitySystem());

			_collectablesSystem = new EcsSystems(_world, _sharedData)
								 .Add(new ApplyCollectablesSystem())
								 .Add(new ApplyExperienceSystem());

			_spawnSystems.Init();

			kdTreesSystem.Init(_spawnSystems);

			_collectablesSystem.Init();
			_updateSystems.Init();
			_skillsSystems.Init();
			_fixedUpdateSystems.Init();
			_destroySystems.Init();

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
			_skillsSystems?.Run();
			_collectablesSystem?.Run();
			_destroySystems?.Run();
		}

		private void FixedUpdate()
		{
			if (!_inited || _gameModel.Paused.Value) return;

			_fixedUpdateSystems?.Run();
		}

		private void OnDestroy()
		{
			_updateCycle?.Dispose();
			_fixedUpdateCycle?.Dispose();

			_updateSystems?.Destroy();
			_fixedUpdateSystems?.Destroy();
			_spawnSystems?.Destroy();
		}

		private void CreateGameModel(IGameModelProvider gameModelProvider, IGameModelSetter gameModelSetter)
		{
			_gameModel = new GameModel();
			_gameModelProvider = gameModelProvider;
			gameModelSetter.Set(_gameModel);
		}

		private void AddCameraToEcsWorld()
		{
			var cameraEntityId = _world.NewEntity();
			cameraEntityId.Add<CameraComponent>();
			ref var cameraTransformComponent = ref cameraEntityId.Add<TransformComponent>();
			cameraTransformComponent.Transform = _camera.transform;
		}
	}
}