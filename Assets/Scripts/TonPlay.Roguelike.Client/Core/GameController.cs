using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Models;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Systems;
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

		[SerializeField]
		private PlayerSpawnConfigProvider _playerSpawnConfigProvider;

		[SerializeField]
		private EnemySpawnConfigProvider _enemySpawnConfigProvider;
		
		private EcsWorld _world;
		private EcsSystems _updateSystems;
		private EcsSystems _fixedUpdateSystems;
		private EcsSystems _spawnSystems;
		private IDisposable _updateCycle;

		private IUIService _uiService;
		private IGameModelProvider _gameModelProvider;
		private IGameModel _gameModel;

		[Inject]
		public void Construct(
			IGameModelProvider gameModelProvider, 
			IGameModelSetter gameModelSetter,
			IUIService uiService)
		{
			_uiService = uiService;
			
			_gameModel = new GameModel();
			_gameModelProvider = gameModelProvider;
			
			gameModelSetter.Set(_gameModel);
			
			Initialize();
		}

		public void Initialize()
		{
			_world = new EcsWorld();

			AddCameraToEcsWorld();

			var sharedData = new SharedData(
				_playerSpawnConfigProvider,
				_enemySpawnConfigProvider,
				_gameModel);

			_spawnSystems = new EcsSystems(_world, sharedData)
						   .Add(new PlayerSpawnSystem())
						   .Add(new BasicEnemySpawnSystem());

			_updateSystems = new EcsSystems(_world, sharedData)
							.Add(new TransformPositionSystem())
							.Add(new CameraMovementSystem())
							.Add(new BasicEnemyMovementTargetSystem())
							.Add(new PlayerCollisionSystem())
							.Add(new ApplyDamageSystem())
							.Add(new ClearUsedEventsSystem())
							.Add(new TransformMovementSystem())
							.Add(new UpdatePlayerModelSystem())
							.Add(new GameOverSystem());

			_fixedUpdateSystems = new EcsSystems(_world, sharedData)
								 .Add(new PlayerInputSystem())
								 .Add(new RigidbodyPositionSystem())
								 .Add(new RigidbodyMovementSystem());

			_spawnSystems.Init();
			_updateSystems.Init();
			_fixedUpdateSystems.Init();

			_updateCycle = Observable.EveryUpdate()
									 .Subscribe(_ => _updateSystems?.Run());
			
			_uiService.Open<GameScreen, IGameScreenContext>(new GameScreenContext());
		}

		private void FixedUpdate()
		{
			_fixedUpdateSystems?.Run();
		}

		private void OnDestroy()
		{
			_updateCycle?.Dispose();
			_updateSystems?.Destroy();
			_fixedUpdateSystems?.Destroy();
			_spawnSystems?.Destroy();
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