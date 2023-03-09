using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class RPGSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly KdTreeStorage _kdTreeStorage;
		private EcsWorld _world;
		private IRPGSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public RPGSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IRPGSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.RPG);

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
						.Filter<RPGSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var rpgPool = _world.GetPool<RPGSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();

			foreach (var entityId in filter)
			{
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var rpg = ref rpgPool.Get(entityId);

				var level = skills.Levels[SkillName.RPG];
				var levelConfig = _config.GetLevelConfig(level);
				
				levelConfig.DamageProvider.DamageMultiplier = damageMultiplier.Value;

				rpg.TimeLeft -= Time.deltaTime;

				if (rpg.TimeLeft <= 0)
				{
					rpg.TimeLeft = levelConfig.Delay;

					var layer = playerPool.Has(entityId)
						? LayerMask.NameToLayer("PlayerProjectile")
						: LayerMask.NameToLayer("EnemyProjectile");

					for (int i = 0; i < levelConfig.ProjectileQuantity; i++)
					{
						CreateProjectile(position.Position, layer, levelConfig.DamageProvider);
					}
				}
			}
		}

		private void CreateProjectile(Vector2 position, int layer, IDamageProvider levelDamageProvider)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}

			var view = poolObject.Object;
			var direction = GetRandomDirection();

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = position;
			transform.right = direction;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, position, direction, collisionLayerMask);

			ref var explodeOnCollision = ref entity.AddOrGet<ExplodeOnCollisionComponent>();
			ref var explodeOnMoveDistance = ref entity.AddOrGet<ExplodeOnMoveDistanceComponent>();

			explodeOnCollision.DamageProvider = levelDamageProvider;
			explodeOnMoveDistance.DamageProvider = levelDamageProvider;

			_kdTreeStorage.AddEntity(entity.Id, position);
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
						.Exc<RPGSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var rpgPool = _world.GetPool<RPGSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					rpgPool.Add(entityId);
				}
			}
		}
	}
}