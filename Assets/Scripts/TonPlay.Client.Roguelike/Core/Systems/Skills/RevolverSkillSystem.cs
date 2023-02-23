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
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class RevolverSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const int MAX_TARGET_QUANTITY = 6;

		private readonly IOverlapExecutor _overlapExecutor;
		private readonly KdTreeStorage _kdTreeStorage;

		private List<int> _overlappedEntities = new List<int>();
		private readonly KDQuery _query = new KDQuery();
		private readonly int[] _cachedTargetEntityIds = new int[MAX_TARGET_QUANTITY];

		private EcsWorld _world;
		private IRevolverSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public RevolverSkillSystem(IOverlapExecutor overlapExecutor, KdTreeStorage kdTreeStorage)
		{
			_overlapExecutor = overlapExecutor;
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IRevolverSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.Revolver);

			_poolIdentity = new ProjectileConfigViewPoolIdentity(_config.ProjectileConfig);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _config.ProjectileConfig.PrefabView, 16);
		}

		public void Run(EcsSystems systems)
		{
			AddSkillComponentIfDoesntExist();
			TrySpawnProjectile();
			SyncSightEffectWithSkillLevel();
		}

		private void SyncSightEffectWithSkillLevel()
		{
			var filter = _world
						.Filter<RevolverSightEffectComponent>()
						.Exc<DeadComponent>()
						.Exc<InactiveComponent>()
						.End();

			var effectPool = _world.GetPool<RevolverSightEffectComponent>();
			var deadPool = _world.GetPool<DeadComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();

			foreach (var entityId in filter)
			{
				ref var effect = ref effectPool.Get(entityId);

				if (deadPool.Has(effect.ParentEntityId))
				{
					continue;
				}

				ref var rotation = ref rotationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(effect.ParentEntityId);

				var level = skills.Levels[SkillName.Revolver];
				var levelConfig = _config.GetLevelConfig(level);

				SetEffectFieldOfView(effect.Effect, levelConfig.FieldOfView, rotation.Direction);
			}
		}

		private void TrySpawnProjectile()
		{
			var filter = _world
						.Filter<RevolverSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<RotationComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<RevolverSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();

			var overlapParams = OverlapParams.Create(_world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());
			overlapParams.Build();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[SkillName.Revolver];
				var levelConfig = _config.GetLevelConfig(level);

				skill.TimeLeft -= Time.deltaTime;

				if (skill.TimeLeft <= 0)
				{
					skill.TimeLeft = levelConfig.ShootDelay;

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					CreateProjectile(position.Position, rotation.Direction, layer, levelConfig, overlapParams);
				}
			}
		}

		private void CreateProjectile(Vector2 position, Vector2 revolverDirection, int layer, IRevolverLevelSkillConfig levelSkillConfig, IOverlapParams overlapParams)
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
			damageOnCollision.DamageProvider = levelSkillConfig.DamageProvider;

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, position);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);

			collisionPool.AddOrGet(entity.Id);
		}

		private Vector3 GetProjectileDirection(Vector2 position, Vector2 revolverDirection, IRevolverLevelSkillConfig levelSkillConfig, IOverlapParams overlapParams)
		{
			var positionPool = _world.GetPool<PositionComponent>();
			var targetsQuantity = _overlapExecutor.Overlap(
				_query,
				position,
				levelSkillConfig.CollisionAreaConfig,
				ref _overlappedEntities,
				levelSkillConfig.CollisionLayerMask,
				overlapParams);

			if (targetsQuantity > 0)
			{
				var currentCachedTargetEntityIdx = 0;
				var halfOfFieldOfView = levelSkillConfig.FieldOfView*0.5f;
				for (var i = 0; i < Mathf.Min(MAX_TARGET_QUANTITY, targetsQuantity); i++)
				{
					var overlappedEntity = _overlappedEntities[i];
					var dirToOverlappedEntity = positionPool.Get(overlappedEntity).Position - position;

					if (Vector2.Angle(dirToOverlappedEntity, revolverDirection) > halfOfFieldOfView)
					{
						continue;
					}

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
					return GetRandomProjectileDirection(revolverDirection, levelSkillConfig);
				}

				var targetIdx = Random.Range(0, currentCachedTargetEntityIdx);
				var targetEntityId = _cachedTargetEntityIds[targetIdx];
				var dirToTarget = positionPool.Get(targetEntityId).Position - position;

				return dirToTarget.normalized;
			}

			return GetRandomProjectileDirection(revolverDirection, levelSkillConfig);
		}

		private static Vector3 GetRandomProjectileDirection(Vector2 revolverDirection, IRevolverLevelSkillConfig levelSkillConfig)
		{
			var randomAngle = Random.Range(
				-levelSkillConfig.FieldOfView*0.5f,
				levelSkillConfig.FieldOfView*0.5f);

			return revolverDirection.Rotate(randomAngle);
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.Exc<RevolverSkill>()
						.End();

			var positionPool = _world.GetPool<PositionComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<RevolverSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					var level = skills.Levels[_config.SkillName];
					var levelConfig = _config.GetLevelConfig(level);
					var effectEntity = CreateEffect(position.Position, entityId, levelConfig.FieldOfView);
				}
			}
		}

		private EcsEntity CreateEffect(Vector2 position, int parentEntityId, float fieldOfView)
		{
			var entity = _world.NewEntity();
			var effect = Object.Instantiate(_config.SightEffectView);
			var transform = effect.transform;
			var right = transform.right;

			transform.position = position;

			SetEffectFieldOfView(effect, fieldOfView, right);

			entity.AddPositionComponent(position);
			entity.AddRotationComponent(right);
			entity.AddTransformComponent(effect.transform);
			entity.AddRevolverSightEffectComponent(effect, parentEntityId);
			entity.AddSyncPositionWithAnotherEntityComponent(parentEntityId);
			entity.AddSyncRotationWithAnotherEntityComponent(parentEntityId);

			return entity;
		}

		private static void SetEffectFieldOfView(RevolverSightEffect effect, float fieldOfView, Vector3 right)
		{
			effect.SetLeftBorderDirection(right.ToVector2XY().Rotate(fieldOfView*-0.5f));
			effect.SetRightBorderDirection(right.ToVector2XY().Rotate(fieldOfView*0.5f));
		}
	}
}