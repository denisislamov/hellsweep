using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using TonPlay.Client.Roguelike.Core.Components.Enemies.ShadowCasterMiniboss;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.ShadowCaster
{
	public class ShadowCasterShootStateSystem : IEcsRunSystem
	{
		private static readonly int s_Shoot = Animator.StringToHash("Shoot");
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			var filter = world.Filter<ShadowCasterComponent>()
							  .Inc<ShadowCasterShootStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<AnimatorComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<ShadowCasterComponent>();
			var followStatePool = world.GetPool<ShadowCasterFollowStateComponent>();
			var shootStatePool = world.GetPool<ShadowCasterShootStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var rotationPool = world.GetPool<RotationComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();

			foreach (var entityId in filter)
			{
				ref var boss = ref bossPool.Get(entityId);
				ref var shoot = ref shootStatePool.Get(entityId);
				ref var animator = ref animatorPool.Get(entityId);
				
				animator.Animator.SetBool(s_Shoot, true);

				if (!shoot.IsInited)
				{
					shoot.IsInited = true;
					shoot.TimeLeft = boss.ShootStateDuration;
					shoot.ShootDelay = boss.InitShootDelay;
				}

				shoot.TimeLeft -= Time.deltaTime;
				shoot.ShootDelay -= Time.deltaTime;

				if (shoot.TimeLeft <= 0)
				{
					shootStatePool.Del(entityId);
					
					followStatePool.Add(entityId);
					continue;
				}

				if (shoot.ShootDelay > 0)
				{
					continue;
				}

				shoot.ShootDelay = boss.ShootDelay;
				shoot.Wave++;

				var position = positionPool.Get(entityId).Position;
				var rotation = rotationPool.Get(entityId).Direction;

				var angle = 120 / boss.ProjectileQuantity;
				var offset = angle * 0.5f;
				for (var i = 0; i < boss.ProjectileQuantity; i++)
				{
					var direction = -rotation.Rotate(offset + angle * (i - boss.ProjectileQuantity * 0.5f));
					
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