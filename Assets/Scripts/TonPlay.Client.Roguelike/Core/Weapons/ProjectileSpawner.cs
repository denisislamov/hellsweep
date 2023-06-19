using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Views;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons
{
	internal static class ProjectileSpawner
	{
		public static EcsEntity SpawnProjectile(
			EcsWorld world,
			IViewPoolObject<ProjectileView> poolObject,
			IProjectileConfig config,
			Vector2 position,
			Vector2 direction,
			int collisionLayerMask,
			int creatorEntityId = EcsEntity.DEFAULT_ID)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample("TonPlay.Client.Roguelike.Core.Weapons.ProjectileSpawner.SpawnProjectile");

			var projectileView = poolObject.Object;
			var projectileEntity = world.NewEntity();
			var projectileViewTransform = projectileView.transform;

			direction.Normalize();

			projectileViewTransform.position = position;
			projectileViewTransform.right = direction;

			ref var projectileViewProviderComponent = ref projectileEntity.Add<ViewProviderComponent>();
			ref var projectileComponent = ref projectileEntity.Add<ProjectileComponent>();
			ref var projectileRotation = ref projectileEntity.Add<RotationComponent>();
			ref var projectilePosition = ref projectileEntity.Add<PositionComponent>();
			ref var projectileMovement = ref projectileEntity.Add<MovementComponent>();
			ref var projectileTransform = ref projectileEntity.Add<TransformComponent>();
			ref var projectileAcceleration = ref projectileEntity.Add<AccelerationComponent>();
			ref var projectileViewPoolObject = ref projectileEntity.Add<ViewPoolObjectComponent>();
			ref var projectileSpeed = ref projectileEntity.AddSpeedComponent(config.MovementConfig.StartSpeed);

			projectileViewProviderComponent.View = projectileView.gameObject;
			projectileComponent.Config = config;
			projectileComponent.CreatorEntityId = creatorEntityId;
			projectileAcceleration.Acceleration = config.MovementConfig.Acceleration;
			projectileRotation.Direction = direction;
			projectilePosition.Position = position;
			projectileMovement.Direction = projectileRotation.Direction;
			projectileTransform.Transform = projectileView.transform;
			projectileViewPoolObject.ViewPoolObject = poolObject;

			projectileEntity.AddLocalPositionComponent(Vector2.zero);
			projectileEntity.AddStackTryApplyDamageComponent();
			projectileEntity.AddBlockApplyDamageTimerComponent();
			projectileEntity.AddLayerComponent(config.Layer);

			if (projectileView.PlayableDirector != null)
			{
				projectileView.PlayableDirector.OptimizedPlay();
				projectileEntity.AddPlayableDirectorComponent(projectileView.PlayableDirector);
			}

			if (config.HasProperty<IDestroyOnTimerProjectileConfigProperty>())
			{
				var destroyOnTimerProjectileConfigProperty = config.GetProperty<IDestroyOnTimerProjectileConfigProperty>();

				ref var projectileDestroyOnTimer = ref projectileEntity.Add<DestroyOnTimerComponent>();
				projectileDestroyOnTimer.TimeLeft = destroyOnTimerProjectileConfigProperty.Timer;
			}
			
			if (config.HasProperty<IExplodeOnTimerProjectileConfigProperty>())
			{
				var property = config.GetProperty<IExplodeOnTimerProjectileConfigProperty>();

				ref var explode = ref projectileEntity.Add<ExplodeOnTimerComponent>();
				explode.CollisionConfig = property.ExplodeProjectileConfig.ExplodeCollisionAreaConfig;
				explode.DamageProvider = property.ExplodeProjectileConfig.DamageProvider;
				explode.CollisionLayerMask = property.ExplodeProjectileConfig.ExplodeCollisionLayerMask;
				explode.TimeLeft = property.Timer;
			}

			if (config.HasProperty<IExplodeOnMoveDistanceProjectileConfigProperty>())
			{
				var explodeOnMoveDistanceProjectileConfigProperty = config.GetProperty<IExplodeOnMoveDistanceProjectileConfigProperty>();

				ref var explodeOnMoveDistance = ref projectileEntity.Add<ExplodeOnMoveDistanceComponent>();
				explodeOnMoveDistance.CollisionConfig = explodeOnMoveDistanceProjectileConfigProperty.ExplodeProjectileConfig.ExplodeCollisionAreaConfig;
				explodeOnMoveDistance.DistanceToExplode = explodeOnMoveDistanceProjectileConfigProperty.Distance;
				explodeOnMoveDistance.StartPosition = position;
				explodeOnMoveDistance.DamageProvider = explodeOnMoveDistanceProjectileConfigProperty.ExplodeProjectileConfig.DamageProvider;
				explodeOnMoveDistance.CollisionLayerMask = explodeOnMoveDistanceProjectileConfigProperty.ExplodeProjectileConfig.ExplodeCollisionLayerMask;
			}

			if (config.HasProperty<IExplodeOnCollisionProjectileConfigProperty>())
			{
				var projectileConfigProperty = config.GetProperty<IExplodeOnCollisionProjectileConfigProperty>();

				ref var explodeOnMoveDistance = ref projectileEntity.Add<ExplodeOnCollisionComponent>();
				explodeOnMoveDistance.CollisionConfig = projectileConfigProperty.ExplodeProjectileConfig.ExplodeCollisionAreaConfig;
				explodeOnMoveDistance.DamageProvider = projectileConfigProperty.ExplodeProjectileConfig.DamageProvider;
				explodeOnMoveDistance.CollisionLayerMask = projectileConfigProperty.ExplodeProjectileConfig.ExplodeCollisionLayerMask;
			}

			if (config.HasProperty<IDamageOnCollisionProjectileConfigProperty>())
			{
				var damageOnCollisionProjectileConfigProperty = config.GetProperty<IDamageOnCollisionProjectileConfigProperty>();

				ref var projectileDamageOnCollision = ref projectileEntity.Add<DamageOnCollisionComponent>();
				projectileDamageOnCollision.DamageProvider = damageOnCollisionProjectileConfigProperty.DamageProvider;
			}

			if (config.HasProperty<IDestroyOnCollisionProjectileConfigProperty>())
			{
				var property = config.GetProperty<IDestroyOnCollisionProjectileConfigProperty>();
				ref var component = ref projectileEntity.Add<DestroyOnCollisionComponent>();
				component.LayerMask = property.LayerMask;
			}

			if (config.HasProperty<IBlockDamageOnCollisionProjectileConfigProperty>())
			{
				projectileEntity.Add<BlockDamageOnCollisionComponent>();
			}

			if (config.HasProperty<IDestroyOnReceiveDamageProjectileConfigProperty>())
			{
				var property = config.GetProperty<IDestroyOnReceiveDamageProjectileConfigProperty>();
				ref var component = ref projectileEntity.Add<DestroyOnReceiveDamageComponent>();
			}

			if (config.HasProperty<ICollisionProjectileConfigProperty>())
			{
				var collisionProjectileConfigProperty = config.GetProperty<ICollisionProjectileConfigProperty>();

				ref var projectileCollision = ref projectileEntity.Add<CollisionComponent>();
				projectileCollision.CollisionArea = CollisionAreaFactory.Create(collisionProjectileConfigProperty.CollisionAreaConfig);
				projectileCollision.LayerMask = collisionLayerMask;

				projectileEntity.AddHasCollidedComponent();
			}

			if (config.HasProperty<IDestroyIfRadiusExceededProjectileConfigProperty>())
			{
				var destroyIfRadiusExceededProjectileConfigProperty = config.GetProperty<IDestroyIfRadiusExceededProjectileConfigProperty>();

				ref var destroyIfDistanceExceededComponent = ref projectileEntity.Add<DestroyIfDistanceExceededComponent>();

				destroyIfDistanceExceededComponent.Distance = destroyIfRadiusExceededProjectileConfigProperty.Distance;
				destroyIfDistanceExceededComponent.StartPosition = position;
			}

			if (config.HasProperty<ISpawnProjectileOnDestroyProjectileConfigProperty>())
			{
				var spawnProjectileOnDestroyProjectileConfigProperty = config.GetProperty<ISpawnProjectileOnDestroyProjectileConfigProperty>();

				projectileEntity.AddSpawnProjectileOnDestroyComponent(
					spawnProjectileOnDestroyProjectileConfigProperty.ProjectileConfig,
					spawnProjectileOnDestroyProjectileConfigProperty.CollisionLayerMask);
			}
			
			if (config.HasProperty<IRicochetOffTheArenaProjectileConfigProperty>())
			{
				var property = config.GetProperty<IRicochetOffTheArenaProjectileConfigProperty>();
				projectileEntity.AddRicochetOffTheArenaComponent(property.CollisionLayerMask);
			}

			if (config.HasProperty<ISpawnEffectOnDestroyProjectileConfigProperty>())
			{
				var property = config.GetProperty<ISpawnEffectOnDestroyProjectileConfigProperty>();
				ref var component = ref projectileEntity.Add<SpawnEffectOnDestroyComponent>();
				component.EffectIdentity = property.EffectViewPoolIdentity;
				component.DestroyTimer = property.DestroyTimer;
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();

			return projectileEntity;
		}
	}
}