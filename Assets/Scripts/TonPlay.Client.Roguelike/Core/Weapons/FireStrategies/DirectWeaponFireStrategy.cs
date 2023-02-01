using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

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

			var pool = _sharedData.CompositeViewPool;

			var collisionLayerMask = _sharedData.CollisionConfigProvider.Get(layer)?.LayerMask ?? 0;

			var config = _sharedData.WeaponConfigProvider.Get(weaponComponent.WeaponConfigId).GetProjectileConfig();

			if (!pool.TryGet(weaponComponent.ProjectileIdentity, out IViewPoolObject<ProjectileView> poolObject))
			{
				Debug.LogWarning("Pool is broken");
				return;
			}

			var entity = ProjectileSpawner.SpawnProjectile(_world, poolObject, config, ownerPosition.Position, ownerRotation.Direction, collisionLayerMask);
		}
	}
}