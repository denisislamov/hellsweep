using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Systems;
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

		public void Initialize()
		{
			_world = new EcsWorld();

			AddCameraToEcsWorld();

			var sharedData = new SharedData(
				_playerSpawnConfigProvider,
				_enemySpawnConfigProvider);

			_spawnSystems = new EcsSystems(_world, sharedData)
						   .Add(new PlayerSpawnSystem())
						   .Add(new BasicEnemySpawnSystem());

			_updateSystems = new EcsSystems(_world)
							.Add(new CameraMovementSystem())
							.Add(new PlayerCollisionEventsSystem())
							.Add(new ApplyDamageSystem())
							.Add(new ClearUsedEventsSystem())
							.Add(new GameOverSystem());

			_fixedUpdateSystems = new EcsSystems(_world)
								 .Add(new PlayerInputSystem())
								 .Add(new EnemyMovementTargetSystem())
								 .Add(new MovementSystem());

			_spawnSystems.Init();
			_updateSystems.Init();
			_fixedUpdateSystems.Init();
		}
		private void Awake()
		{
			Initialize();
		}

		private void Update()
		{
			_updateSystems?.Run();
		}

		private void FixedUpdate()
		{
			_fixedUpdateSystems?.Run();
		}

		private void OnDestroy()
		{
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