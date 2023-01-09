using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Roguelike.Client.Core.Weapons.FireStrategies
{
	public class DirectWeaponFireStrategy : IWeaponFireStrategy
	{
		private readonly EcsWorld _world;
		private readonly ISharedData _sharedData;

		public DirectWeaponFireStrategy(EcsWorld world, ISharedData sharedData)
		{
			_world = world;
			_sharedData = sharedData;
		}

		public void Fire(ref WeaponComponent weaponComponent)
		{
			var ownerId = weaponComponent.OwnerEntityId;
			var rotationPool = _world.GetPool<RotationComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			
			var playerPool = _world.GetPool<PlayerComponent>();

			if (!positionPool.Has(ownerId) ||
				!rotationPool.Has(ownerId))
			{
				return;
			}

			var layer = playerPool.Has(ownerId)
				? LayerMask.NameToLayer("PlayerProjectile")
				: LayerMask.NameToLayer("EnemyProjectile");

			ref var ownerRotation = ref rotationPool.Get(ownerId);
			ref var ownerPosition = ref positionPool.Get(ownerId);

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var config = _sharedData.WeaponConfigProvider.Get(weaponComponent.WeaponConfigId).GetProjectileConfig();
			var projectileEntity = _world.NewEntity();
			var projectileView = Object.Instantiate(config.PrefabView);
			var projectileViewTransform = projectileView.transform;

			var gameObject = projectileView.gameObject;
			gameObject.name = string.Format("{0} (entity: {1})", gameObject.name, projectileEntity.Id.ToString());
			
			projectileViewTransform.position = ownerPosition.Position;
			projectileViewTransform.right = ownerRotation.Direction;

			ref var projectileViewProviderComponent = ref projectileEntity.Add<ViewProviderComponent>();
			ref var projectileComponent = ref projectileEntity.Add<ProjectileComponent>();
			ref var projectileCollision = ref projectileEntity.Add<CollisionComponent>();
			ref var projectileRotation = ref projectileEntity.Add<RotationComponent>();
			ref var projectilePosition = ref projectileEntity.Add<PositionComponent>();
			ref var projectileMovement = ref projectileEntity.Add<MovementComponent>();
			ref var projectileTransform = ref projectileEntity.Add<TransformComponent>();
			ref var projectileSpeed = ref projectileEntity.Add<SpeedComponent>();
			ref var projectileDamageOnCollision = ref projectileEntity.Add<DamageOnCollisionComponent>();

			projectileEntity.Add<DestroyOnCollisionComponent>();

			projectileViewProviderComponent.View = projectileView.gameObject;
			projectileComponent.Config = config;
			projectileSpeed.Speed = config.StartSpeed;
			projectileRotation.Direction = ownerRotation.Direction;
			projectilePosition.Position = ownerPosition.Position;
			projectileMovement.Vector = projectileRotation.Direction;
			projectileTransform.Transform = projectileView.transform;
			projectileCollision.CollisionAreaConfig = config.CollisionAreaConfig; 
			projectileCollision.LayerMask = collisionLayerMask;
			projectileDamageOnCollision.Damage = config.Damage;
		}
	}
}