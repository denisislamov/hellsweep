using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeHorseman;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossJadeHorseman
{
	public class BossJadeHorsemanShootStateSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			var filter = world.Filter<BossJadeHorsemanComponent>()
							  .Inc<BossJadeHorsemanShootStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossJadeHorsemanComponent>();
			var tankStatePool = world.GetPool<BossJadeHorsemanTankStateComponent>();
			var shootStatePool = world.GetPool<BossJadeHorsemanShootStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var speedPool = world.GetPool<SpeedComponent>();

			foreach (var entityId in filter)
			{
				ref var boss = ref bossPool.Get(entityId);
				ref var shoot = ref shootStatePool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);

				if (!shoot.IsInited)
				{
					shoot.IsInited = true;
					shoot.TimeLeft = boss.ShootStateDuration;
					
					speed.InitialSpeed = 0f;
				}

				shoot.TimeLeft -= Time.deltaTime;
				shoot.ShootDelay -= Time.deltaTime;

				if (shoot.TimeLeft <= 0)
				{
					shootStatePool.Del(entityId);

					speed.InitialSpeed = 0f;
					
					tankStatePool.Add(entityId);
					continue;
				}

				if (shoot.ShootDelay > 0)
				{
					continue;
				}

				shoot.ShootDelay = boss.ShootDelay;
				shoot.Wave++;

				var position = positionPool.Get(entityId).Position;

				var angle = 360 / boss.ProjectileQuantity;
				var offset = angle * 0.5f * shoot.Wave;
				for (var i = 0; i < boss.ProjectileQuantity; i++)
				{
					var direction = Vector2.right.Rotate(offset + angle * i);
					
					CreateProjectile(position, direction, sharedData, boss.ProjectileConfig);
				}
			}
		}
		
		private void CreateProjectile(
			Vector2 position, 
			Vector2 direction, 
			ISharedData sharedData, 
			IProjectileConfig projectileConfig)
		{
			var world = sharedData.MainWorld;
			var pool = sharedData.CompositeViewPool;
			
			var collisionLayerMask = sharedData.CollisionConfigProvider.Get(projectileConfig.Layer)?.LayerMask ?? 0;

			if (pool.TryGet<ProjectileView>(projectileConfig.Identity, out var poolObject))
			{
				var entity = ProjectileSpawner.SpawnProjectile(world, poolObject, projectileConfig, position, direction, collisionLayerMask);
			}
		}
	}
}