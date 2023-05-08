using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Views;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class ShurikenSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int MAX_TARGET_QUANTITY = 6;

		private readonly IOverlapExecutor _overlapExecutor;
		private readonly KdTreeStorage _kdTreeStorage;

		private List<int> _overlappedEntities = new List<int>();
		private readonly KDQuery _query = new KDQuery();
		private readonly int[] _cachedTargetEntityIds = new int[MAX_TARGET_QUANTITY];

		private EcsWorld _world;
		private IShurikenSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public ShurikenSkillSystem(IOverlapExecutor overlapExecutor, KdTreeStorage kdTreeStorage)
		{
			_overlapExecutor = overlapExecutor;
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IShurikenSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.Kunai);

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
						.Filter<ShurikenSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<RotationComponent>()
						.Inc<BaseDamageComponent>()
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<ShurikenSkill>();
			var attackPool = _world.GetPool<AttackEvent>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();
			var baseDamagePool = _world.GetPool<BaseDamageComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();

			var overlapParams = OverlapParams.Create(_world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());
			overlapParams.Build();

			foreach (var entityId in filter)
			{
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
				ref var baseDamage = ref baseDamagePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[SkillName.Kunai];
				var levelConfig = _config.GetLevelConfig(level);

				levelConfig.DamageProvider.DamageMultiplier = damageMultiplier.Value;

				skill.TimeLeft -= Time.deltaTime;

				if (skill.TimeLeft <= 0)
				{
					skill.TimeLeft = levelConfig.ShootDelay;
					
					attackPool.Add(entityId);

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					CreateProjectile(position.Position, rotation.Direction, layer, levelConfig, overlapParams, baseDamage.Value);
				}
			}
		}

		private void CreateProjectile(Vector2 position,
									  Vector2 revolverDirection,
									  int layer,
									  IShurikenLevelSkillConfig levelSkillConfig,
									  IOverlapParams overlapParams,
									  float baseDamage)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}

			var view = poolObject.Object;
			var direction = GetProjectileDirection(position, revolverDirection, levelSkillConfig, overlapParams);

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = position;
			transform.right = direction;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, position, direction, collisionLayerMask);
			var damageOnCollisionPool = _world.GetPool<DamageOnCollisionComponent>();
			var collisionPool = _world.GetPool<CollisionComponent>();

			ref var damageOnCollision = ref damageOnCollisionPool.AddOrGet(entity.Id);
			damageOnCollision.DamageProvider = levelSkillConfig.DamageProvider.Clone().AddDamageValue(baseDamage);

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, position);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);

			collisionPool.AddOrGet(entity.Id);
		}

		private Vector3 GetProjectileDirection(Vector2 position, Vector2 shurikenDirection, IShurikenLevelSkillConfig levelSkillConfig, IOverlapParams overlapParams)
		{
			var positionPool = _world.GetPool<PositionComponent>();
			var targetsQuantity = _overlapExecutor.Overlap(
				_query,
				position,
				CollisionAreaFactory.Create(levelSkillConfig.CollisionAreaConfig),
				ref _overlappedEntities,
				levelSkillConfig.CollisionLayerMask,
				overlapParams);

			if (targetsQuantity > 0)
			{
				var currentCachedTargetEntityIdx = 0;
				for (var i = 0; i < Mathf.Min(MAX_TARGET_QUANTITY, targetsQuantity); i++)
				{
					_cachedTargetEntityIds[currentCachedTargetEntityIdx] = _overlappedEntities[i];
					currentCachedTargetEntityIdx++;

					if (currentCachedTargetEntityIdx >= MAX_TARGET_QUANTITY)
					{
						break;
					}
				}

				_overlappedEntities.Clear();

				if (currentCachedTargetEntityIdx == 0)
				{
					return GetRandomProjectileDirection(shurikenDirection, levelSkillConfig);
				}

				var targetIdx = Random.Range(0, currentCachedTargetEntityIdx);
				var targetEntityId = _cachedTargetEntityIds[targetIdx];
				var dirToTarget = positionPool.Get(targetEntityId).Position - position;

				return dirToTarget.normalized;
			}

			return GetRandomProjectileDirection(shurikenDirection, levelSkillConfig);
		}

		private static Vector3 GetRandomProjectileDirection(Vector2 shurikenDirection, IShurikenLevelSkillConfig levelSkillConfig)
		{
			var randomAngle = Random.Range(0, 360f);

			return shurikenDirection.Rotate(randomAngle);
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.Exc<ShurikenSkill>()
						.End();

			var positionPool = _world.GetPool<PositionComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<ShurikenSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
				}
			}
		}
	}
}