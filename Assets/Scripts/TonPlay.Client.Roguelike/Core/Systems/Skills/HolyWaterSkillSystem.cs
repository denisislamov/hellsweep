using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class HolyWaterSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IHolyWaterSkillConfig _config;
		private ICompositeViewPool _pool;
		private ISharedData _sharedData;
		private ProjectileConfigViewPoolIdentity _throwablePoolIdentity;
		private ProjectileConfigViewPoolIdentity _areaPoolIdentity;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IHolyWaterSkillConfig) _sharedData.SkillsConfigProvider.Get(SkillName.Molotov);

			_throwablePoolIdentity = new ProjectileConfigViewPoolIdentity(_config.BottleProjectileConfig);
			_areaPoolIdentity = new ProjectileConfigViewPoolIdentity(_config.DamagingAreaProjectileConfig);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_throwablePoolIdentity, _config.BottleProjectileConfig.PrefabView, 16);
			_pool.Add(_areaPoolIdentity, _config.DamagingAreaProjectileConfig.PrefabView, 16);
		}

		public void Run(EcsSystems systems)
		{
			AddSkillComponentIfDoesntExist();
			TryPrepareToSpawnThrowableProjectile();
			TrySpawnThrowableProjectile();
		}

		private void TryPrepareToSpawnThrowableProjectile()
		{
			var filter = _world
						.Filter<HolyWaterSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<HolyWaterSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[_config.SkillName];
				var levelConfig = _config.GetLevelConfig(level);

				skill.RefreshLeftTime -= Time.deltaTime;

				if (skill.RefreshLeftTime <= 0)
				{
					skill.RefreshLeftTime = levelConfig.Cooldown;

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					var mainDirection = GetRandomDirection();
					var angleInterval = 360f/levelConfig.Quantity;

					for (var i = 0; i < levelConfig.Quantity; i++)
					{
						var entity = _world.NewEntity();
						var spawnAngle = angleInterval*i;
						var direction = mainDirection.Rotate(spawnAngle);

						entity.AddBottleOfHolyWaterProjectileComponent();
						entity.AddPrepareToSpawnAfterTimerComponent(_config.DelayBetweenThrowingProjectiles);
						entity.AddPrepareToSpawnBottleOfHolyWaterProjectileComponent(levelConfig, layer);
						entity.AddRotationComponent(direction);
						entity.AddPositionComponent(position.Position);
					}
				}
			}
		}

		private void TrySpawnThrowableProjectile()
		{
			var filter = _world
						.Filter<BottleOfHolyWaterProjectileComponent>()
						.Inc<PrepareToSpawnAfterTimerComponent>()
						.Inc<RotationComponent>()
						.Inc<PositionComponent>()
						.End();

			var timerPool = _world.GetPool<PrepareToSpawnAfterTimerComponent>();
			var preparePool = _world.GetPool<PrepareToSpawnBottleOfHolyWaterProjectileComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();
			var positionPool = _world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var timer = ref timerPool.Get(entityId);
				ref var prepare = ref preparePool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				timer.TimeLeft -= Time.deltaTime;

				if (timer.TimeLeft <= 0)
				{
					CreateThrowableProjectile(position.Position, rotation.Direction, prepare.Layer, prepare.Config);

					_world.DelEntity(entityId);
				}
			}
		}

		private void CreateThrowableProjectile(Vector2 position, Vector2 direction, int layer, IHolyWaterSkillLevelConfig levelConfig)
		{
			if (!_pool.TryGet<ProjectileView>(_throwablePoolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}
			var view = poolObject.Object;

			var damageOnCollisionPool = _world.GetPool<DamageOnCollisionComponent>();

			var spawnPosition = position;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = spawnPosition;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.BottleProjectileConfig, spawnPosition, direction, collisionLayerMask);
			entity.AddBottleOfHolyWaterProjectileComponent();

			ref var damageOnCollision = ref damageOnCollisionPool.AddOrGet(entity.Id);
			damageOnCollision.DamageProvider = levelConfig.DamageProvider;
		}

		private Vector2 GetRandomDirection()
		{
			return Random.onUnitSphere;
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<HolyWaterSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<HolyWaterSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					skillPool.Add(entityId);
				}
			}
		}
	}
}