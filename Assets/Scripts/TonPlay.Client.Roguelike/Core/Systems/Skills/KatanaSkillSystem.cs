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
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class KatanaSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		private EcsWorld _world;
		private IKatanaSkillConfig _config;
		private ICompositeViewPool _pool;
		private ISharedData _sharedData;
		private ProjectileConfigViewPoolIdentity _poolIdentity;

		public KatanaSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IKatanaSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.Katana);

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
						.Filter<KatanaSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<RotationComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<KatanaSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[_config.SkillName];
				var levelConfig = _config.GetLevelConfig(level);

				skill.RefreshLeftTime -= Time.deltaTime;

				if (skill.RefreshLeftTime <= 0)
				{
					if (skill.SpawnQuantity == 0)
					{
						var dirX = rotation.Direction.x > 0 ? 1 : -1;
						var mainDirection = new Vector2(dirX, 0);

						skill.SpawnQuantity = levelConfig.ProjectileQuantity;
						skill.SelectedDirection = mainDirection;
					}

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					var direction = skill.SelectedDirection*((levelConfig.ProjectileQuantity - skill.SpawnQuantity)%2 == 0 ? 1 : -1);

					CreateThrowableProjectile(position.Position, direction, entityId, layer, levelConfig);

					skill.SpawnQuantity--;
					skill.RefreshLeftTime = skill.SpawnQuantity == 0
						? levelConfig.Cooldown
						: levelConfig.ShootDelay;
				}
			}
		}

		private void CreateThrowableProjectile(Vector2 position, Vector2 direction, int entityId, int layer, IKatanaLevelSkillConfig levelConfig)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}

			var view = poolObject.Object;

			var damageOnCollisionPool = _world.GetPool<DamageOnCollisionComponent>();
			var localPositionPool = _world.GetPool<LocalPositionComponent>();

			var spawnPosition = position;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, spawnPosition, direction, collisionLayerMask);
			entity.AddKatanaSplashProjectileComponent();
			entity.AddMoveInLocalSpaceOfEntityComponent(entityId);

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, spawnPosition);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);

			ref var damageOnCollision = ref damageOnCollisionPool.AddOrGet(entity.Id);
			damageOnCollision.DamageProvider = levelConfig.DamageProvider;

			ref var localPosition = ref localPositionPool.AddOrGet(entity.Id);
			localPosition.Position = levelConfig.SpawnOffset*direction;

			var transform = view.transform;
			transform.position = spawnPosition + localPosition.Position;
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<KatanaSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<KatanaSkill>();

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