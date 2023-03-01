using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossWorm
{
	public class BossWormTankStateSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossWormComponent>()
							  .Inc<BossWormTankStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossWormComponent>();
			var followStatePool = world.GetPool<BossWormFollowStateComponent>();
			var tankStatePool = world.GetPool<BossWormTankStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
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
				
				if (!tank.IsInited)
				{
					tank.IsInited = true;
					tank.State = BossWormTankState.Prepare;
					tank.TimeLeft = boss.TankPreparingDuration;
				}

				tank.TimeLeft -= Time.deltaTime;
				
				switch (tank.State)
				{
					case BossWormTankState.Prepare:
					{
						RunPreparingState(positionPool, entityId, movementPool, playerPosition, ref tank, ref boss, ref speed);
						break;
					}
					case BossWormTankState.Stop:
					{
						RunStoppingState(ref tank, ref speed, tankStatePool, entityId, followStatePool);
						break;
					}
					case BossWormTankState.Run:
					{
						RunRunningState(ref tank, ref boss, ref speed);
						break;
					}
				}
			}
		}
		
		private static void RunRunningState(
			ref BossWormTankStateComponent tank, 
			ref BossWormComponent boss,
			ref SpeedComponent speed)
		{
			speed.Speed = boss.TankSpeed;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossWormTankState.Stop;
			tank.TimeLeft = boss.TankStoppingDuration;
		}
		
		private static void RunStoppingState(
			ref BossWormTankStateComponent tank, 
			ref SpeedComponent speed,
			EcsPool<BossWormTankStateComponent> tankStatePool, 
			int entityId, 
			EcsPool<BossWormFollowStateComponent> followStatePool)
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
			ref BossWormTankStateComponent tank, 
			ref BossWormComponent boss,
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

			tank.State = BossWormTankState.Run;
			tank.TimeLeft = boss.TankRunningDuration;
		}
	}
}