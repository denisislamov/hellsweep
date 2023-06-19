using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeHorseman;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossJadeHorseman
{
	public class BossJadeHorsemanTankStateSystem : IEcsRunSystem
	{
		private static readonly int s_Tank = Animator.StringToHash("Tank");
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossJadeHorsemanComponent>()
							  .Inc<BossJadeHorsemanTankStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Inc<AnimatorComponent>()
							  .Inc<DamageOnCollisionComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossJadeHorsemanComponent>();
			var shootStatePool = world.GetPool<BossJadeHorsemanShootStateComponent>();
			var tankStatePool = world.GetPool<BossJadeHorsemanTankStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();
			var damagePool = world.GetPool<DamageOnCollisionComponent>();
			var speedPool = world.GetPool<SpeedComponent>();

			var playerPosition = Vector2.zero;			
			
			foreach (var entityId in world.Filter<PlayerComponent>().End())
			{
				playerPosition = positionPool.Get(entityId).Position;
				break;
			}

			foreach (var entityId in filter)
			{
				ref var boss = ref bossPool.Get(entityId);
				ref var tank = ref tankStatePool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);
				ref var damage = ref damagePool.Get(entityId);
				ref var animator = ref animatorPool.Get(entityId);

				if (!tank.IsInited)
				{
					tank.IsInited = true;
					tank.Count = boss.TankCount;
					tank.State = BossJadeHorsemanTankState.Prepare;
					tank.TimeLeft = boss.TankPreparingDuration;
				}

				tank.TimeLeft -= Time.deltaTime;
				
				switch (tank.State)
				{
					case BossJadeHorsemanTankState.Prepare:
					{
						RunPreparingState(positionPool, entityId, movementPool, playerPosition, ref tank, ref boss, ref speed);
						break;
					}
					case BossJadeHorsemanTankState.Stop:
					{
						animator.Animator.SetBool(s_Tank, false);
						damage.DamageProvider = boss.ShootStateDamageProvider;
						
						RunStoppingState(ref boss, ref tank, ref speed, tankStatePool, entityId, shootStatePool);
						break;
					}
					case BossJadeHorsemanTankState.Run:
					{
						animator.Animator.SetBool(s_Tank, true);

						damage.DamageProvider = boss.TankStateDamageProvider;

						RunRunningState(ref tank, ref boss, ref speed);
						break;
					}
				}
			}
		}
		
		private static void RunRunningState(
			ref BossJadeHorsemanTankStateComponent tank, 
			ref BossJadeHorsemanComponent boss,
			ref SpeedComponent speed)
		{
			speed.InitialSpeed = boss.TankSpeed;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossJadeHorsemanTankState.Stop;
			tank.TimeLeft = boss.TankStoppingDuration;
		}
		
		private static void RunStoppingState(
			ref BossJadeHorsemanComponent boss, 
			ref BossJadeHorsemanTankStateComponent tank, 
			ref SpeedComponent speed,
			EcsPool<BossJadeHorsemanTankStateComponent> tankStatePool, 
			int entityId, 
			EcsPool<BossJadeHorsemanShootStateComponent> shootStatePool)
		{
			speed.InitialSpeed = 0;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.Count--;

			if (tank.Count > 0)
			{
				tank.State = BossJadeHorsemanTankState.Prepare;
				tank.TimeLeft = boss.TankPreparingDuration;
				return;
			}

			tankStatePool.Del(entityId);
			shootStatePool.Add(entityId);
		}
		
		private static void RunPreparingState(
			EcsPool<PositionComponent> positionPool, 
			int entityId, 
			EcsPool<MovementComponent> movementPool, 
			Vector2 playerPosition, 
			ref BossJadeHorsemanTankStateComponent tank, 
			ref BossJadeHorsemanComponent boss,
			ref SpeedComponent speed)
		{
			var position = positionPool.Get(entityId).Position;
			ref var movement = ref movementPool.Get(entityId);

			movement.Direction = playerPosition - position;
			movement.Direction.Normalize();

			speed.InitialSpeed = 0;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossJadeHorsemanTankState.Run;
			tank.TimeLeft = boss.TankRunningDuration;
		}
	}
}