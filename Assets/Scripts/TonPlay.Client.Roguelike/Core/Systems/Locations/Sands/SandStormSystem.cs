using System;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Locations.Sands;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Locations.Sands;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Locations.Sands
{
	public class SandStormSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const float SPEED = 8f;
		private const float SPAWN_DISTANCE = 30f;
		private const float DESTROY_DISTANCE = 30f;

		private readonly float[] _timings;
		private int _currentTimingIdx = 0;
		
		private readonly SandStormEffect _sandsParticles;
		private readonly IViewPoolIdentity _sandsIdentity;

		public SandStormSystem(SandStormEffect sandsParticles)
		{
			_sandsParticles = sandsParticles;
			_sandsIdentity = new GameObjectViewPoolIdentity(_sandsParticles.gameObject);

			_timings = new[] { 1f, 2f, 3f, 4f, 4.8f, 6f, 7f, 8f, 9.8f, 11f, 12f, 13.5f, 14.8f };
		}
		
		public void Init(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			sharedData.CompositeViewPool.Add<SandStormEffect>(_sandsIdentity, _sandsParticles, 4);
		}
		
		private void CreateSandStorm(EcsSystems systems, Vector2 direction)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var entity = world.NewEntity();

			if (!sharedData.CompositeViewPool.TryGet<SandStormEffect>(_sandsIdentity, out var viewPoolObject))
			{
				return;
			}

			var view = viewPoolObject.Object.gameObject;
			entity.AddPositionComponent(sharedData.PlayerPositionProvider.Position + -direction * SPAWN_DISTANCE);
			entity.AddTransformComponent(view.transform);
			entity.AddSpeedComponent(SPEED);
			entity.AddMovementComponent();
			entity.AddPoolObjectComponent(viewPoolObject);
			entity.Add<SandStormComponent>();

			var movementPool = world.GetPool<MovementComponent>();
			ref var movement = ref movementPool.Get(entity.Id);
			movement.Direction = direction;
			
			view.gameObject.SetActive(true);
		}

		public void Run(EcsSystems systems)
		{
			SyncStormPositionWithPlayerPosition(systems);
			TryToCreateStormOnTiming(systems);
			TryToDespawnStorm(systems);
		}
		
		private void TryToCreateStormOnTiming(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();

			if (TimeSpan.FromSeconds(sharedData.GameModel.GameTimeInSeconds.Value) < TimeSpan.FromMinutes(_timings[_currentTimingIdx]))
			{
				return;
			}
			
			_currentTimingIdx++;
			CreateSandStorm(systems, Vector2.left * (UnityEngine.Random.Range(0f, 1f) > 0.5f ? 1 : -1));
		}

		private static void SyncStormPositionWithPlayerPosition(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var filter = world.Filter<SandStormComponent>().Inc<MovementComponent>().Inc<PositionComponent>().End();
			var positionPool = world.GetPool<PositionComponent>();
			var stormPool = world.GetPool<SandStormComponent>();

			foreach (var entityIdx in filter)
			{
				ref var position = ref positionPool.Get(entityIdx);

				position.Position.y = sharedData.PlayerPositionProvider.Position.y;

				if (Mathf.Abs(position.Position.x - sharedData.PlayerPositionProvider.Position.x) < 1f)
				{
					ref var storm = ref stormPool.Get(entityIdx);
					storm.ReachedPlayer = true;
				}
			}
		}
		
		private void TryToDespawnStorm(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var filter = world.Filter<SandStormComponent>().Inc<MovementComponent>().Inc<PositionComponent>().End();
			var positionPool = world.GetPool<PositionComponent>();
			var stormPool = world.GetPool<SandStormComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityIdx in filter)
			{
				ref var position = ref positionPool.Get(entityIdx);
				ref var storm = ref stormPool.Get(entityIdx);

				if (!storm.ReachedPlayer || 
					Mathf.Abs(position.Position.x - sharedData.PlayerPositionProvider.Position.x) < DESTROY_DISTANCE)
				{
					continue;
				}

				destroyPool.Add(entityIdx);
			}
		}
	}
}