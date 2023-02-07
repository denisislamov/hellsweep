using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
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
			int collisionLayerMask)
		{
			var projectileView = poolObject.Object;
			var projectileEntity = world.NewEntity();
			var projectileViewTransform = projectileView.transform;

			var gameObject = projectileView.gameObject;
			gameObject.name = string.Format("{0} (entity: {1})", config.PrefabView.gameObject.name, projectileEntity.Id.ToString());

			projectileViewTransform.position = position;
			projectileViewTransform.right = direction;

			ref var projectileViewProviderComponent = ref projectileEntity.Add<ViewProviderComponent>();
			ref var projectileComponent = ref projectileEntity.Add<ProjectileComponent>();
			ref var projectileRotation = ref projectileEntity.Add<RotationComponent>();
			ref var projectilePosition = ref projectileEntity.Add<PositionComponent>();
			ref var projectileMovement = ref projectileEntity.Add<MovementComponent>();
			ref var projectileTransform = ref projectileEntity.Add<TransformComponent>();
			ref var projectileSpeed = ref projectileEntity.Add<SpeedComponent>();
			ref var projectileAcceleration = ref projectileEntity.Add<AccelerationComponent>();
			ref var projectileViewPoolObject = ref projectileEntity.Add<ViewPoolObjectComponent>();
			
			projectileViewProviderComponent.View = projectileView.gameObject;
			projectileComponent.Config = config;
			projectileSpeed.Speed = config.MovementConfig.StartSpeed;
			projectileAcceleration.Acceleration = config.MovementConfig.Acceleration;
			projectileRotation.Direction = direction;
			projectilePosition.Position = position;
			projectileMovement.Direction = projectileRotation.Direction;
			projectileTransform.Transform = projectileView.transform;
			projectileViewPoolObject.ViewPoolObject = poolObject;

			if (config.TryGetProperty<IDestroyOnTimerProjectileConfigProperty>(out var destroyOnTimerProjectileConfigProperty))
			{
				ref var projectileDestroyOnTimer = ref projectileEntity.Add<DestroyOnTimerComponent>();
				projectileDestroyOnTimer.TimeLeft = destroyOnTimerProjectileConfigProperty.Timer;
			}
			
			if (config.TryGetProperty<IExplodeOnMoveDistanceProjectileConfigProperty>(out var explodeOnMoveDistanceProjectileConfigProperty))
			{
				ref var explodeOnMoveDistance = ref projectileEntity.Add<ExplodeOnMoveDistanceComponent>();
				explodeOnMoveDistance.CollisionConfig = explodeOnMoveDistanceProjectileConfigProperty.ExplodeCollisionAreaConfig;
				explodeOnMoveDistance.DistanceToExplode = explodeOnMoveDistanceProjectileConfigProperty.Distance;
				explodeOnMoveDistance.StartPosition = position;
				explodeOnMoveDistance.Damage = explodeOnMoveDistanceProjectileConfigProperty.Damage;
			}

			if (config.TryGetProperty<IDamageOnCollisionProjectileConfigProperty>(out var damageOnCollisionProjectileConfigProperty))
			{
				ref var projectileDamageOnCollision = ref projectileEntity.Add<DamageOnCollisionComponent>();
				projectileDamageOnCollision.DamageProvider = damageOnCollisionProjectileConfigProperty.DamageProvider;
			}
			
			if (config.TryGetProperty<IDestroyOnCollisionProjectileConfigProperty>(out var destroyOnCollisionProjectileConfigProperty))
			{
				projectileEntity.Add<DestroyOnCollisionComponent>();
			}
			
			if (config.TryGetProperty<IBlockDamageOnCollisionProjectileConfigProperty>(out var blockDamageOnCollisionProjectileConfigProperty))
			{
				projectileEntity.Add<BlockDamageOnCollisionComponent>();
			}
			
			if (config.TryGetProperty<ICollisionProjectileConfigProperty>(out var collisionProjectileConfigProperty))
			{
				ref var projectileCollision = ref projectileEntity.Add<CollisionComponent>();
				projectileCollision.CollisionAreaConfig = collisionProjectileConfigProperty.CollisionAreaConfig;
				projectileCollision.LayerMask = collisionLayerMask;
			}
			
			if (config.TryGetProperty<IDestroyIfRadiusExceededProjectileConfigProperty>(out var destroyIfRadiusExceededProjectileConfigProperty))
			{
				ref var destroyIfDistanceExceededComponent = ref projectileEntity.Add<DestroyIfDistanceExceededComponent>();

				destroyIfDistanceExceededComponent.Distance = destroyIfRadiusExceededProjectileConfigProperty.Distance;
				destroyIfDistanceExceededComponent.StartPosition = position;
			}
			
			if (config.TryGetProperty<ISpawnProjectileOnDestroyProjectileConfigProperty>(out var spawnProjectileOnDestroyProjectileConfigProperty))
			{
				projectileEntity.AddSpawnProjectileOnDestroyComponent(
					spawnProjectileOnDestroyProjectileConfigProperty.ProjectileConfig,
					spawnProjectileOnDestroyProjectileConfigProperty.CollisionLayerMask);
			}

			return projectileEntity;
		}
	}
}