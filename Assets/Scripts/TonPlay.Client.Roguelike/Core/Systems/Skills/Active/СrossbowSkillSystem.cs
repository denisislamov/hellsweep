using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
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
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class CrossbowSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		private EcsWorld _world;
		private ICrossbowSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public CrossbowSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (ICrossbowSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.Crossbow);

			_poolIdentity = new ProjectileConfigViewPoolIdentity(_config.ProjectileConfig);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _config.ProjectileConfig.PrefabView, 128);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			AddSkillComponentIfDoesntExist();
			TrySpawnProjectile();
			SyncSightEffectWithSkillLevel();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void SyncSightEffectWithSkillLevel()
		{
			var filter = _world
						.Filter<CrossbowSightEffectComponent>()
						.Exc<DeadComponent>()
						.Exc<InactiveComponent>()
						.End();

			var effectPool = _world.GetPool<CrossbowSightEffectComponent>();
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

				var level = skills.Levels[SkillName.Crossbow];
				var levelConfig = _config.GetLevelConfig(level);

				SetEffectFieldOfView(effect.Effect, levelConfig.FieldOfView, rotation.Direction);
			}
		}

		private void TrySpawnProjectile()
		{
			var filter = _world
						.Filter<CrossbowSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<RotationComponent>()
						.Inc<BaseDamageComponent>()
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<CrossbowSkill>();
			var attackPool = _world.GetPool<AttackEvent>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();
			var baseDamagePool = _world.GetPool<BaseDamageComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();

			foreach (var entityId in filter)
			{
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
				ref var baseDamage = ref baseDamagePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				var level = skills.Levels[SkillName.Crossbow];
				var levelConfig = _config.GetLevelConfig(level);

				levelConfig.DamageProvider.DamageMultiplier = damageMultiplier.Value;

				skill.TimeLeft -= Time.deltaTime;

				if (skill.TimeLeft <= 0)
				{
					attackPool.Add(entityId);
					
					skill.TimeLeft = levelConfig.ShootDelay;

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					var angleStep = levelConfig.FieldOfView/(levelConfig.ProjectileQuantity - 1);
					for (var idx = 0; idx < levelConfig.ProjectileQuantity; idx++)
					{
						CreateProjectile(idx, position.Position, rotation.Direction, angleStep, layer, levelConfig, baseDamage.Value);
					}
				}
			}
		}

		private void CreateProjectile(
			int idx, 
			Vector2 position, 
			Vector2 crossbowDirection, 
			float angleStep, 
			int layer, 
			ICrossbowLevelSkillConfig levelSkillConfig, 
			float baseDamage)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}

			var halfFieldOfView = levelSkillConfig.FieldOfView*0.5f;
			var view = poolObject.Object;
			var direction = crossbowDirection.Rotate(-halfFieldOfView + angleStep*idx);

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = position;
			transform.right = direction;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, position, direction, collisionLayerMask);
			var damageOnCollisionPool = _world.GetPool<DamageOnCollisionComponent>();
			var collisionPool = _world.GetPool<CollisionComponent>();

			ref var damageOnCollision = ref damageOnCollisionPool.AddOrGet(entity.Id);
			damageOnCollision.DamageProvider = levelSkillConfig.DamageProvider.Clone().AddDamageValue(baseDamage);

			collisionPool.AddOrGet(entity.Id);

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, position);
			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.Exc<CrossbowSkill>()
						.End();

			var positionPool = _world.GetPool<PositionComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<CrossbowSkill>();

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
			entity.AddCrossbowSightEffectComponent(effect, parentEntityId);
			entity.AddSyncPositionWithAnotherEntityComponent(parentEntityId);
			entity.AddSyncRotationWithAnotherEntityComponent(parentEntityId);

			return entity;
		}

		private static void SetEffectFieldOfView(CrossbowSightEffect effect, float fieldOfView, Vector3 right)
		{
			effect.SetLeftBorderDirection(right.ToVector2XY().Rotate(fieldOfView*-0.5f));
			effect.SetRightBorderDirection(right.ToVector2XY().Rotate(fieldOfView*0.5f));
		}
	}
}