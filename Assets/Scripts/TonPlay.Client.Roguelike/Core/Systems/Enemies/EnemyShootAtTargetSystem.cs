using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Views;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class EnemyShootAtTargetSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<EnemyComponent>()
						.Inc<TargetComponent>()
						.Inc<ShootProjectileAtTargetComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var positionPool = world.GetPool<PositionComponent>();
			var targetPool = world.GetPool<TargetComponent>();
			var shootPool = world.GetPool<ShootProjectileAtTargetComponent>();
			var deadPool = world.GetPool<DeadComponent>();

			var sharedData = systems.GetShared<ISharedData>();
			var pool = sharedData.CompositeViewPool;
			var collisionConfigProvider = sharedData.CollisionConfigProvider;

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var target = ref targetPool.Get(entityId);
				ref var shoot = ref shootPool.Get(entityId);

				if (deadPool.Has(target.EntityId) || !positionPool.Has(target.EntityId))
				{
					continue;
				}

				ref var targetPosition = ref positionPool.Get(target.EntityId);

				var direction = targetPosition.Position - position.Position;
				direction.Normalize();

				var sqrMinDistance = shoot.MinDistanceTargetToShoot*shoot.MinDistanceTargetToShoot;
				var sqrMaxDistance = shoot.MaxDistanceTargetToShoot*shoot.MaxDistanceTargetToShoot;

				shoot.TimeLeft -= Time.deltaTime;

				if (shoot.TimeLeft > 0 || direction.sqrMagnitude < sqrMinDistance || direction.sqrMagnitude > sqrMaxDistance)
				{
					continue;
				}

				if (!pool.TryGet<ProjectileView>(shoot.ProjectileIdentity, out var poolObject))
				{
					continue;
				}

				var collisionLayerMask = collisionConfigProvider.Get(shoot.Layer)?.LayerMask ?? 0;

				var projectile = ProjectileSpawner.SpawnProjectile(
					world,
					poolObject,
					shoot.ProjectileConfig,
					position.Position,
					direction,
					collisionLayerMask);

				shoot.TimeLeft = shoot.Rate;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}