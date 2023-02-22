using Leopotam.EcsLite;
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

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class GuardianSkillSystem  : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;

		private EcsWorld _world;
		private IGuardianSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public GuardianSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();
			
			_world = systems.GetWorld();
			_config = (IGuardianSkillConfig) _sharedData.SkillsConfigProvider.Get(SkillName.Guardian);

			_poolIdentity = new ProjectileConfigViewPoolIdentity(_config.ProjectileConfig);
			
			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _config.ProjectileConfig.PrefabView, 16);
		}
		
		public void Run(EcsSystems systems)
		{
			AddSkillComponentIfDoesntExist();
			TrySpawnProjectile();
			TryDestroyProjectiles();
		}
		
		private void TryDestroyProjectiles()
		{
			var filter = _world
						.Filter<GuardianProjectileComponent>()
						.Exc<DestroyComponent>()
						.Exc<InactiveComponent>()
						.End();

			var projectilePool = _world.GetPool<GuardianProjectileComponent>();
			var destroyPool = _world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				ref var projectile = ref projectilePool.Get(entityId);
				projectile.ActiveLeftTime -= Time.deltaTime;

				if (projectile.ActiveLeftTime <= 0)
				{
					destroyPool.Add(entityId);
				}
			}
		}

		private void TrySpawnProjectile()
		{
			var filter = _world
						.Filter<GuardianSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var rpgPool = _world.GetPool<GuardianSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref rpgPool.Get(entityId);

				var level = skills.Levels[_config.SkillName];
				var levelConfig = _config.GetLevelConfig(level);
				
				skill.RefreshLeftTime -= Time.deltaTime;

				if (skill.RefreshLeftTime <= 0)
				{
					skill.RefreshLeftTime = levelConfig.Cooldown + levelConfig.ActiveTime;
					
					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					var angleInterval = 360f / levelConfig.Quantity;
					for (var i = 0; i < levelConfig.Quantity; i++)
					{
						var spawnAngle = angleInterval * i;

						CreateProjectile(position.Position, spawnAngle, layer, entityId, levelConfig);
					}
				}
			}
		}
		
		private void CreateProjectile(Vector2 position, float angle, int layer, int playerEntityId, IGuardianSkillLevelConfig levelConfig)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}
			var view = poolObject.Object;
			
			var speedPool = _world.GetPool<SpeedComponent>();
			var movementPool = _world.GetPool<MovementComponent>();

			var direction = GetRandomDirection();
			var spawnPosition = position + Vector2.right.Rotate(angle) * levelConfig.Radius;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = spawnPosition;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, spawnPosition, direction, collisionLayerMask);
			
			entity.AddSpinAroundEntityPositionComponent(playerEntityId, levelConfig.Radius, angle);
			entity.AddGuardianProjectileComponent(levelConfig.ActiveTime);
			
			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, spawnPosition);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);
			entity.AddDrawDebugKdTreePositionComponent();
			
			ref var damageOnCollisionComponent = ref entity.AddOrGet<DamageOnCollisionComponent>();
			ref var speed = ref speedPool.Get(entity.Id);
			
			speed.Speed = GetSpeed(levelConfig);
			damageOnCollisionComponent.DamageProvider = levelConfig.DamageProvider;

			if (movementPool.Has(entity.Id))
			{
				movementPool.Del(entity.Id);
			}
		}

		private Vector2 GetRandomDirection()
		{
			return Random.insideUnitCircle;
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Exc<DeadComponent>()
						.Exc<GuardianSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<GuardianSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					skillPool.Add(entityId);
				}
			}
		}
		
		private static float GetSpeed(IGuardianSkillLevelConfig levelConfig) => levelConfig.Speed;
	}
}