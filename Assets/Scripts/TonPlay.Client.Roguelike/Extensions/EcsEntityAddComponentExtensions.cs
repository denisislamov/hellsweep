using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Extensions
{
	public static class EcsEntityAddComponentExtensions
	{
		public static ref T AddOrGet<T>(this EcsEntity entity) where T : struct
		{
			if (entity.Has<T>())
			{
				return ref entity.Get<T>();
			}
			else
			{
				return ref entity.Add<T>();
			}
		}
		
		public static ref ViewPoolObjectComponent AddPoolObjectComponent(this EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var viewPoolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			viewPoolObjectComponent.ViewPoolObject = viewPoolObject;

			return ref viewPoolObjectComponent;
		}

		public static ref CollectableComponent AddCollectableComponent(this EcsEntity entity, ICollectableConfig config)
		{
			ref var collectable = ref entity.Add<CollectableComponent>();
			collectable.Type = config.Type;
			collectable.Value = config.Value;

			return ref collectable;
		}

		public static ref LayerComponent AddLayerComponent(this EcsEntity entity, int layer)
		{
			ref var layerComponent = ref entity.Add<LayerComponent>();
			layerComponent.Layer = layer;
			return ref layerComponent;
		}

		public static ref PositionComponent AddPositionComponent(this EcsEntity entity, Vector2 position)
		{
			ref var positionComponent = ref entity.Add<PositionComponent>();
			positionComponent.Position = position;
			return ref positionComponent;
		}

		public static ref TransformComponent AddTransformComponent(this EcsEntity entity, Transform transform)
		{
			ref var transformComponent = ref entity.Add<TransformComponent>();
			transformComponent.Transform = transform;
			return ref transformComponent;
		}

		public static ref ViewProviderComponent AddViewProviderComponent(this EcsEntity entity, GameObject gameObject)
		{
			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = gameObject;
			return ref viewProviderComponent;
		}

		public static ref StickToLocationBlockComponent AddStickToLocationBlockComponent(this EcsEntity entity)
		{
			ref var stickToLocationBlockComponent = ref entity.Add<StickToLocationBlockComponent>();
			return ref stickToLocationBlockComponent;
		}

		public static ref MagnetCollectableComponent AddMagnetCollectableComponent(this EcsEntity entity, IMagnetCollectableConfig config)
		{
			ref var component = ref entity.Add<MagnetCollectableComponent>();
			component.Config = config;
			return ref component;
		}

		public static ref BombCollectableComponent AddBombCollectableComponent(this EcsEntity entity, IBombCollectableConfig config)
		{
			ref var component = ref entity.Add<BombCollectableComponent>();
			component.Config = config;
			return ref component;
		}

		public static ref SkillsComponent AddSkillsComponent(this EcsEntity entity)
		{
			ref var skillsComponent = ref entity.Add<SkillsComponent>();
			skillsComponent.Levels = new Dictionary<SkillName, int>();
			return ref skillsComponent;
		}

		public static ref RotationComponent AddRotationComponent(this EcsEntity entity, Vector2 direction)
		{
			ref var rotationComponent = ref entity.Add<RotationComponent>();
			rotationComponent.Direction = direction;
			return ref rotationComponent;
		}

		public static ref SpeedComponent AddSpeedComponent(this EcsEntity entity, IMovementConfig movementConfig)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = movementConfig.StartSpeed;
			return ref speedComponent;
		}

		public static ref MovementComponent AddMovementComponent(this EcsEntity entity)
		{
			return ref entity.Add<MovementComponent>();
		}

		public static ref RigidbodyComponent AddRigidbodyComponent(this EcsEntity entity, Rigidbody2D rigidbody)
		{
			ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
			rigidbodyComponent.Rigidbody = rigidbody;
			return ref rigidbodyComponent;
		}

		public static ref HealthComponent AddHealthComponent(this EcsEntity entity, float startHealth, float maxHealth)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = startHealth;
			healthComponent.MaxHealth = maxHealth;
			return ref healthComponent;
		}

		public static ref ProfileExperienceComponent AddProfileExperienceComponent(this EcsEntity entity)
		{
			return ref entity.Add<ProfileExperienceComponent>();
		}

		public static ref GoldComponent AddGoldComponent(this EcsEntity entity)
		{
			return ref entity.Add<GoldComponent>();
		}

		public static ref ExperienceComponent AddExperienceComponent(this EcsEntity entity, float value, float maxValue, int level)
		{
			ref var exp = ref entity.Add<ExperienceComponent>();
			exp.Value = value;
			exp.MaxValue = maxValue;
			exp.Level = level;
			return ref exp;
		}

		public static ref MagnetizableComponent AddMagnetizableComponent(this EcsEntity entity)
		{
			return ref entity.Add<MagnetizableComponent>();
		}

		public static ref ExplosionComponent AddExplosionComponent(this EcsEntity entity, IDamageProvider damageProvider, ICollisionAreaConfig collisionAreaConfig, int layerMask)
		{
			ref var explosion = ref entity.Add<ExplosionComponent>();
			explosion.CollisionAreaConfig = collisionAreaConfig;
			explosion.DamageProvider = damageProvider;
			explosion.LayerMask = layerMask;
			return ref explosion;
		}

		public static ref SpinAroundEntityPositionComponent AddSpinAroundEntityPositionComponent(this EcsEntity entity, int spinAroundEntityId, float radius, float lastAngle)
		{
			ref var component = ref entity.Add<SpinAroundEntityPositionComponent>();
			component.SpinAroundEntityId = spinAroundEntityId;
			component.Radius = radius;
			component.LastAngle = lastAngle;
			return ref component;
		}

		public static ref GuardianProjectileComponent AddGuardianProjectileComponent(this EcsEntity entity, float leftTime)
		{
			ref var component = ref entity.Add<GuardianProjectileComponent>();
			component.ActiveLeftTime = leftTime;
			return ref component;
		}

		public static ref CollisionComponent AddCollisionComponent(this EcsEntity entity, ICollisionAreaConfig collisionAreaConfig, int layerMask)
		{
			ref var component = ref entity.Add<CollisionComponent>();
			component.CollisionAreaConfig = collisionAreaConfig;
			component.LayerMask = layerMask;

			if (collisionAreaConfig.DoNotInitiateCollisionOverlap)
			{
				entity.Add<DoNotInitiateCollisionOverlap>();
			}
			
			return ref component;
		}

		public static ref DamageOnDistanceChangeComponent AddDamageOnDistanceChangeComponent(this EcsEntity entity, IDamageProvider damageProvider, Vector2 lastDamagePosition)
		{
			ref var component = ref entity.Add<DamageOnDistanceChangeComponent>();
			component.LastDamagePosition = lastDamagePosition;
			component.DamageProvider = damageProvider;
			return ref component;
		}
		
		public static ref DamageOnCollisionComponent AddDamageOnCollisionComponent(this EcsEntity entity, IDamageProvider damageProvider)
		{
			ref var component = ref entity.Add<DamageOnCollisionComponent>();
			component.DamageProvider = damageProvider;
			return ref component;
		}
		
		public static ref DestroyOnCollisionComponent AddDestroyOnCollisionComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<DestroyOnCollisionComponent>();
			return ref component;
		}

		public static ref BrickProjectileComponent AddBrickProjectileComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<BrickProjectileComponent>();
			return ref component;
		}

		public static ref InvertMovementAxisOnSpeedInversionComponent AddInvertMovementAxisOnSpeedInversionComponent(this EcsEntity entity, bool invertX, bool invertY)
		{
			ref var component = ref entity.Add<InvertMovementAxisOnSpeedInversionComponent>();
			component.AxisX = invertX;
			component.AxisY = invertY;
			return ref component;
		}

		public static ref SyncRotationWithPositionDifferenceComponent AddSyncRotationWithPositionDifferenceComponent(this EcsEntity entity, Vector2 position)
		{
			ref var component = ref entity.Add<SyncRotationWithPositionDifferenceComponent>();
			component.LastPosition = position;
			return ref component;
		}

		public static ref ForcefieldDeviceEffectComponent AddForcefieldDeviceEffectComponent(this EcsEntity entity, int parentEntityId)
		{
			ref var component = ref entity.Add<ForcefieldDeviceEffectComponent>();
			component.ParentEntityId = parentEntityId;
			return ref component;
		}

		public static ref SyncPositionWithAnotherEntityComponent AddSyncPositionWithAnotherEntityComponent(this EcsEntity entity, int parentEntityId)
		{
			ref var component = ref entity.Add<SyncPositionWithAnotherEntityComponent>();
			component.ParentEntityId = parentEntityId;
			return ref component;
		}

		public static ref SyncRotationWithAnotherEntityComponent AddSyncRotationWithAnotherEntityComponent(this EcsEntity entity, int parentEntityId)
		{
			ref var component = ref entity.Add<SyncRotationWithAnotherEntityComponent>();
			component.ParentEntityId = parentEntityId;
			return ref component;
		}

		public static ref RevolverSightEffectComponent AddRevolverSightEffectComponent(this EcsEntity entity, RevolverSightEffect effect, int parentEntityId)
		{
			ref var component = ref entity.Add<RevolverSightEffectComponent>();
			component.Effect = effect;
			component.ParentEntityId = parentEntityId;
			return ref component;
		}

		public static ref CrossbowSightEffectComponent AddCrossbowSightEffectComponent(this EcsEntity entity, CrossbowSightEffect effect, int parentEntityId)
		{
			ref var component = ref entity.Add<CrossbowSightEffectComponent>();
			component.Effect = effect;
			component.ParentEntityId = parentEntityId;
			return ref component;
		}

		public static ref BottleOfHolyWaterProjectileComponent AddBottleOfHolyWaterProjectileComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<BottleOfHolyWaterProjectileComponent>();
			return ref component;
		}

		public static ref PrepareToSpawnAfterTimerComponent AddPrepareToSpawnAfterTimerComponent(this EcsEntity entity, float seconds)
		{
			ref var component = ref entity.Add<PrepareToSpawnAfterTimerComponent>();
			component.TimeLeft = seconds;
			return ref component;
		}

		public static ref PrepareToSpawnBottleOfHolyWaterProjectileComponent AddPrepareToSpawnBottleOfHolyWaterProjectileComponent(this EcsEntity entity, IHolyWaterSkillLevelConfig config, int layer)
		{
			ref var component = ref entity.Add<PrepareToSpawnBottleOfHolyWaterProjectileComponent>();
			component.Config = config;
			component.Layer = layer;
			return ref component;
		}

		public static ref SpawnProjectileOnDestroyComponent AddSpawnProjectileOnDestroyComponent(this EcsEntity entity, IProjectileConfig config, int layer)
		{
			ref var component = ref entity.Add<SpawnProjectileOnDestroyComponent>();
			component.ProjectileConfig = config;
			component.CollisionLayerMask = layer;
			return ref component;
		}

		public static ref KatanaSplashProjectileComponent AddKatanaSplashProjectileComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<KatanaSplashProjectileComponent>();
			return ref component;
		}

		public static ref MoveInLocalSpaceOfEntityComponent AddMoveInLocalSpaceOfEntityComponent(this EcsEntity entity, int entityId)
		{
			ref var component = ref entity.Add<MoveInLocalSpaceOfEntityComponent>();
			component.EntityId = entityId;
			return ref component;
		}

		public static ref LocalPositionComponent AddLocalPositionComponent(this EcsEntity entity, Vector2 position)
		{
			ref var component = ref entity.Add<LocalPositionComponent>();
			component.Position = position;
			return ref component;
		}

		public static ref ShootProjectileAtTargetComponent AddShootProjectileAtTargetComponent(
			this EcsEntity entity,
			IProjectileConfig projectileConfig,
			int layer, 
			float rate,
			float minDistanceTargetToShoot,
			float maxDistanceTargetToShoot)
		{
			ref var component = ref entity.Add<ShootProjectileAtTargetComponent>();
			component.ProjectileConfig = projectileConfig;
			component.ProjectileIdentity = projectileConfig.Identity;
			component.Layer = layer;
			component.Rate = rate;
			component.MinDistanceTargetToShoot = minDistanceTargetToShoot;
			component.MaxDistanceTargetToShoot = maxDistanceTargetToShoot;
			return ref component;
		}

		public static ref EnemyTargetComponent AddOrUpdateEnemyTargetComponent(
			this EcsEntity entity,
			int entityId)
		{
			ref var component = ref entity.AddOrGet<EnemyTargetComponent>();
			component.EntityId = entityId;
			return ref component;
		}
		
		public static ref HasCollidedComponent AddHasCollidedComponent(
			this EcsEntity entity)
		{
			ref var component = ref entity.AddOrGet<HasCollidedComponent>();
			component.CollidedEntityIds = new List<int>();
			return ref component;
		}
		
		public static ref StackTryApplyDamageComponent AddStackTryApplyDamageComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<StackTryApplyDamageComponent>();
			component.Stack = new Stack<TryApplyDamageComponent>();
			return ref component;
		}

		public static ref BlockApplyDamageTimerComponent AddBlockApplyDamageTimerComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<BlockApplyDamageTimerComponent>();
			component.Blocked = new Dictionary<string, Dictionary<int, ReactiveProperty<float>>>();
			return ref component;
		}
		
		public static ref ShowAppliedDamageIndicatorComponent AddShowAppliedDamageIndicatorComponent(this EcsEntity entity)
		{
			ref var component = ref entity.Add<ShowAppliedDamageIndicatorComponent>();
			return ref component;
		}
	}
}