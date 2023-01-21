using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons
{
	internal static class ProjectileSpawner
	{
		public static EcsEntity SpawnProjectile(EcsWorld world, IViewPoolObject<ProjectileView> poolObject, IProjectileConfig config, Vector2 position, Vector2 direction, int collisionLayerMask)
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

			projectileEntity.Add<DestroyOnCollisionComponent>();

			projectileViewProviderComponent.View = projectileView.gameObject;
			projectileComponent.Config = config;
			projectileSpeed.Speed = config.MovementConfig.StartSpeed;
			projectileAcceleration.Acceleration = config.MovementConfig.Acceleration;
			projectileRotation.Direction = direction;
			projectilePosition.Position = position;
			projectileMovement.Vector = projectileRotation.Direction;
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
				ref var projectileCollision = ref projectileEntity.Add<CollisionComponent>();

				projectileDamageOnCollision.Damage = damageOnCollisionProjectileConfigProperty.Damage;
				projectileCollision.CollisionAreaConfig = damageOnCollisionProjectileConfigProperty.CollisionAreaConfig;
				projectileCollision.LayerMask = collisionLayerMask;
			}

			return projectileEntity;
		}
	}
}