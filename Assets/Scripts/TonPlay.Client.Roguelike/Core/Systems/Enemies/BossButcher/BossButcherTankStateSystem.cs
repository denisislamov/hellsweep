using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossButcher
{
	public class BossButcherTankStateSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossButcherComponent>()
							  .Inc<BossButcherTankStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossButcherComponent>();
			var followStatePool = world.GetPool<BossButcherFollowStateComponent>();
			var tankStatePool = world.GetPool<BossButcherTankStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var speedPool = world.GetPool<SpeedComponent>();
			var damagePool = world.GetPool<DamageOnCollisionComponent>();

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
				
				if (!tank.IsInited)
				{
					ref var damage = ref damagePool.Get(entityId);

					tank.IsInited = true;
					tank.State = BossButcherTankState.Prepare;
					tank.TimeLeft = boss.TankPreparingDuration;

					damage.DamageProvider = boss.TankDamageProvider;
				}

				tank.TimeLeft -= Time.deltaTime;
				
				switch (tank.State)
				{
					case BossButcherTankState.Prepare:
					{
						RunPreparingState(positionPool, entityId, movementPool, playerPosition, ref tank, ref boss, ref speed);
						break;
					}
					case BossButcherTankState.Stop:
					{
						RunStoppingState(ref tank, ref speed, tankStatePool, entityId, followStatePool);
						break;
					}
					case BossButcherTankState.Run:
					{
						RunRunningState(ref tank, ref boss, ref speed);
						break;
					}
				}
			}
		}
		
		private static void RunRunningState(
			ref BossButcherTankStateComponent tank, 
			ref BossButcherComponent boss,
			ref SpeedComponent speed)
		{
			speed.Speed = boss.TankSpeed;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossButcherTankState.Stop;
			tank.TimeLeft = boss.TankStoppingDuration;
		}
		
		private static void RunStoppingState(
			ref BossButcherTankStateComponent tank, 
			ref SpeedComponent speed,
			EcsPool<BossButcherTankStateComponent> tankStatePool, 
			int entityId, 
			EcsPool<BossButcherFollowStateComponent> followStatePool)
		{
			speed.Speed = 0;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tankStatePool.Del(entityId);
			followStatePool.Add(entityId);
		}
		
		private static void RunPreparingState(
			EcsPool<PositionComponent> positionPool, 
			int entityId, 
			EcsPool<MovementComponent> movementPool, 
			Vector2 playerPosition, 
			ref BossButcherTankStateComponent tank, 
			ref BossButcherComponent boss,
			ref SpeedComponent speed)
		{
			var position = positionPool.Get(entityId).Position;
			ref var movement = ref movementPool.Get(entityId);

			movement.Direction = playerPosition - position;
			movement.Direction.Normalize();

			speed.Speed = 0;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossButcherTankState.Run;
			tank.TimeLeft = boss.TankRunningDuration;
		}
	}
}