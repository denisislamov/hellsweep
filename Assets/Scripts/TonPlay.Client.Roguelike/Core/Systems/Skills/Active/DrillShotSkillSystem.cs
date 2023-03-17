using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
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

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class DrillShotSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const float CHECK_DELAY = 0.2f;

		private readonly KdTreeStorage _kdTreeStorage;

		private EcsWorld _world;
		private IDrillShotSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public DrillShotSkillSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IDrillShotSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.DrillShot);

			_poolIdentity = new ProjectileConfigViewPoolIdentity(_config.ProjectileConfig);

			_pool = _sharedData.CompositeViewPool;
			_pool.Add(_poolIdentity, _config.ProjectileConfig.PrefabView, 16);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			AddSkillComponentIfDoesntExist();
			TrySpawnProjectile();
			TryRicochetProjectile();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void TrySpawnProjectile()
		{
			var filter = _world
						.Filter<DrillShotSkill>()
						.Inc<SkillsComponent>()
						.Inc<PositionComponent>()
						.Inc<DamageMultiplierComponent>()
						.Inc<SkillDurationMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<DrillShotSkill>();
			var playerPool = _world.GetPool<PlayerComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();
			var skillDurationMultiplierPool = _world.GetPool<SkillDurationMultiplierComponent>();

			foreach (var entityId in filter)
			{
				ref var skillDurationMultiplier = ref skillDurationMultiplierPool.Get(entityId);
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

					var duration = levelConfig.ActiveTime * skillDurationMultiplier.Value;
					
					CreateProjectile(position.Position, layer, levelConfig, entityId, duration);

					skill.SpawnQuantity--;
					skill.RefreshLeftTime = skill.SpawnQuantity == 0
						? levelConfig.Cooldown + duration
						: _config.DelayBetweenSpawn;
				}
			}
		}

		private void TryRicochetProjectile()
		{
			var filter = _world
						.Filter<DrillShotProjectileComponent>()
						.Inc<MovementComponent>()
						.Inc<RotationComponent>()
						.Exc<DestroyComponent>()
						.End();

			var drillShotPool = _world.GetPool<DrillShotProjectileComponent>();
			var movementPool = _world.GetPool<MovementComponent>();
			var rotationPool = _world.GetPool<RotationComponent>();
			var positionPool = _world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				var drillShot = drillShotPool.Get(entityId);
				var creatorPosition = positionPool.Get(drillShot.CreatorEntityId).Position;
				var drillShotPosition = positionPool.Get(entityId).Position;

				var zonePosition = creatorPosition + drillShot.Config.FlyingZone.position;
				var zone = new Rect(zonePosition, drillShot.Config.FlyingZone.size);

				var insideRect = RectContains(zone, zonePosition, drillShotPosition);


				// var leftDownCorner = new Vector2(
				// 	zonePosition.x + zone.width  * -0.5f,
				// 	zonePosition.y + zone.height * -0.5f);
				//
				// var leftUpCorner = new Vector2(
				// 	zonePosition.x + zone.width  * -0.5f,
				// 	zonePosition.y + zone.height * 0.5f);
				//
				// var rightDownCorner = new Vector2(
				// 	zonePosition.x + zone.width  * 0.5f,
				// 	zonePosition.y + zone.height * -0.5f);
				//
				// var rightUpCorner = new Vector2(
				// 	zonePosition.x + zone.width  * 0.5f,
				// 	zonePosition.y + zone.height * 0.5f);
				//
				// Debug.DrawLine(leftUpCorner, rightDownCorner, Color.green, Time.deltaTime);
				// Debug.DrawLine(leftDownCorner, rightUpCorner, Color.green, Time.deltaTime);

				if (insideRect)
				{
					drillShot.LastInsideRectState = true;
					continue;
				}

				if (!drillShot.LastInsideRectState)
				{
					continue;
				}

				drillShot.LastInsideRectState = false;

				ref var movement = ref movementPool.Get(entityId);
				ref var rotation = ref rotationPool.Get(entityId);

				var bounds = new Bounds(zonePosition, zone.size);
				var closestPoint = bounds.ClosestPoint(drillShotPosition).ToVector2XY();
				var boundNormal = closestPoint - drillShotPosition;

				// Debug.DrawLine(closestPoint, drillShotPosition, Color.red, Time.deltaTime);
				// Debug.DrawRay(drillShotPosition, boundNormal, Color.blue, Time.deltaTime);

				var dot = Vector2.Dot(movement.Direction, boundNormal);

				if (dot >= 0)
				{
					continue;
				}

				movement.Direction = new Vector2(
					boundNormal.x != 0 ? -movement.Direction.x : movement.Direction.x,
					boundNormal.y != 0 ? -movement.Direction.y : movement.Direction.y);
				rotation.Direction = movement.Direction;
			}
		}

		private void CreateProjectile(
			Vector2 position,
			int layer,
			IDrillShotSkillLevelConfig levelConfig,
			int creatorEntityId,
			float duration)
		{
			if (!_pool.TryGet<ProjectileView>(_poolIdentity, out var poolObject))
			{
				Debug.LogWarning("Some shit is happenin here");
				return;
			}
			var view = poolObject.Object;

			var direction = GetRandomDirection();
			var spawnPosition = position;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var transform = view.transform;
			transform.position = spawnPosition;

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, _config.ProjectileConfig, spawnPosition, direction, collisionLayerMask);
			entity.AddDrillShotProjectileComponent(creatorEntityId, _config, levelConfig);

			ref var damageOnCollisionComponent = ref entity.AddOrGet<DamageOnCollisionComponent>();
			ref var destroyOnTimerComponent = ref entity.Get<DestroyOnTimerComponent>();
			ref var speed = ref entity.AddOrGet<SpeedComponent>();

			destroyOnTimerComponent.TimeLeft = duration;
			damageOnCollisionComponent.DamageProvider = levelConfig.DamageProvider;
			speed.Speed = levelConfig.Speed;

			var treeIndex = _kdTreeStorage.AddEntity(entity.Id, spawnPosition);

			entity.AddKdTreeElementComponent(_kdTreeStorage, treeIndex);
			entity.AddDrawDebugKdTreePositionComponent();
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
						.Exc<DrillShotSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<DrillShotSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					skillPool.Add(entityId);
				}
			}
		}

		private bool RectContains(Rect rect, Vector2 rectPosition, Vector2 dotPosition)
		{
			var xMin = rectPosition.x + rect.size.x*-0.5f;
			var xMax = rectPosition.x + rect.size.x*0.5f;
			var yMin = rectPosition.y + rect.size.y*-0.5f;
			var yMax = rectPosition.y + rect.size.y*0.5f;

			return dotPosition.x >= xMin && dotPosition.x <= xMax &&
				   dotPosition.y >= yMin && dotPosition.y <= yMax;
		}
	}
}