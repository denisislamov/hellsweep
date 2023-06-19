using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeWarrior;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossJadeWarrior
{
	public class BossJadeWarriorTankStateSystem : IEcsRunSystem
	{
		private static readonly int s_Attack = Animator.StringToHash("Attack");
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossJadeWarriorComponent>()
							  .Inc<BossJadeWarriorTankStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Inc<AnimatorComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossJadeWarriorComponent>();
			var followStatePool = world.GetPool<BossJadeWarriorFollowStateComponent>();
			var tankStatePool = world.GetPool<BossJadeWarriorTankStateComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var speedPool = world.GetPool<SpeedComponent>();
			var damagePool = world.GetPool<DamageOnCollisionComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();

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
				ref var animator = ref animatorPool.Get(entityId);
				
				if (!tank.IsInited)
				{
					ref var damage = ref damagePool.Get(entityId);

					tank.IsInited = true;
					tank.State = BossJadeWarriorTankState.Prepare;
					tank.TimeLeft = boss.TankPreparingDuration;

					damage.DamageProvider = boss.TankDamageProvider;
				}

				tank.TimeLeft -= Time.deltaTime;
				
				switch (tank.State)
				{
					case BossJadeWarriorTankState.Prepare:
					{
						RunPreparingState(positionPool, entityId, movementPool, playerPosition, ref tank, ref boss, ref speed);
						break;
					}
					case BossJadeWarriorTankState.Stop:
					{
						animator.Animator.SetBool(s_Attack, false);

						RunStoppingState(ref tank, ref speed, tankStatePool, entityId, followStatePool);
						break;
					}
					case BossJadeWarriorTankState.Run:
					{
						animator.Animator.SetBool(s_Attack, true);
						
						RunRunningState(ref tank, ref boss, ref speed);
						break;
					}
				}
			}
		}
		
		private static void RunRunningState(
			ref BossJadeWarriorTankStateComponent tank, 
			ref BossJadeWarriorComponent boss,
			ref SpeedComponent speed)
		{
			speed.InitialSpeed = boss.TankSpeed;

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossJadeWarriorTankState.Stop;
			tank.TimeLeft = boss.TankStoppingDuration;
		}
		
		private static void RunStoppingState(
			ref BossJadeWarriorTankStateComponent tank, 
			ref SpeedComponent speed,
			EcsPool<BossJadeWarriorTankStateComponent> tankStatePool, 
			int entityId, 
			EcsPool<BossJadeWarriorFollowStateComponent> followStatePool)
		{
			speed.InitialSpeed = 0;

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
			ref BossJadeWarriorTankStateComponent tank, 
			ref BossJadeWarriorComponent boss,
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

			tank.State = BossJadeWarriorTankState.Run;
			tank.TimeLeft = boss.TankRunningDuration;
		}
	}
}