using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class BrickSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;
		private EcsWorld _world;
		private IBrickSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public BrickSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IBrickSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.Brick);

			_poolIdentity = new ProjectileConfigViewPoolIdentity(_config.ProjectileConfig);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _config.ProjectileConfig.PrefabView, 16);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			AddSkillComponentIfDoesntExist();
			TrySpawnProjectile();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void TrySpawnProjectile()
		{
			var filter = _world
						.Filter<BrickSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<BrickSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();

			foreach (var entityId in filter)
			{
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[_config.SkillName];
				var levelConfig = _config.GetLevelConfig(level);

				levelConfig.DamageProvider.DamageMultiplier = damageMultiplier.Value;

				skill.RefreshLeftTime -= Time.deltaTime;

				if (skill.RefreshLeftTime <= 0)
				{
					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					if (skill.SpawnQuantity == 0)
					{
						skill.SpawnQuantity = levelConfig.Quantity;
					}

					CreateProjectile(position.Position, layer, levelConfig);

					skill.SpawnQuantity--;
					skill.RefreshLeftTime = skill.SpawnQuantity == 0
						? levelConfig.Cooldown
						: _config.DelayBetweenSpawn;
				}
			}
		}

		private void CreateProjectile(Vector2 position, int layer, IBrickSkillLevelConfig levelConfig)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}
			var view = poolObject.Object;

			var speedPool = _world.GetPool<SpeedComponent>();
			var movementPool = _world.GetPool<MovementComponent>();
			var accelerationPool = _world.GetPool<AccelerationComponent>();

			var direction = GetRandomDirection();
			var spawnPosition = position;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = spawnPosition;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, spawnPosition, direction, collisionLayerMask);
			entity.AddBrickProjectileComponent();
			entity.AddInvertMovementAxisOnSpeedInversionComponent(true, false);
			entity.AddSyncRotationWithPositionDifferenceComponent(position - direction);

			ref var damageOnCollisionComponent = ref entity.AddOrGet<DamageOnCollisionComponent>();
			ref var speed = ref speedPool.AddOrGet(entity.Id);
			ref var acceleration = ref accelerationPool.AddOrGet(entity.Id);

			damageOnCollisionComponent.DamageProvider = levelConfig.DamageProvider;

			speed.Speed = (_config.DistanceToThrow - acceleration.Acceleration*Mathf.Pow(_config.TimeToReachDistance, 2)/2f)
						  /_config.TimeToReachDistance;

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, spawnPosition);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);
			entity.AddDrawDebugKdTreePositionComponent();
		}

		private Vector2 GetRandomDirection()
		{
			return new Vector2(Random.Range(-1f, 1f)*0.4f, 1f).normalized;
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<BrickSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<BrickSkill>();

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