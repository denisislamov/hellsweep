using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossUnicorn;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossUnicorn
{
	public class BossUnicornTankStateSystem : IEcsRunSystem
	{
		private static readonly int s_Attack = Animator.StringToHash("Attack");
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossUnicornComponent>()
							  .Inc<BossUnicornTankStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Inc<AnimatorComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossUnicornComponent>();
			var followStatePool = world.GetPool<BossUnicornFollowStateComponent>();
			var tankStatePool = world.GetPool<BossUnicornTankStateComponent>();
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
					tank.State = BossUnicornTankState.Prepare;
					tank.TimeLeft = boss.TankPreparingDuration;

					damage.DamageProvider = boss.TankDamageProvider;
				}

				tank.TimeLeft -= Time.deltaTime;

				switch (tank.State)
				{
					case BossUnicornTankState.Prepare:
					{
						RunPreparingState(positionPool, entityId, movementPool, playerPosition, ref tank, ref boss, ref speed);
						break;
					}
					case BossUnicornTankState.Stop:
					{
						RunStoppingState(ref boss, ref tank, ref speed, ref animator, tankStatePool, entityId, followStatePool);
						break;
					}
					case BossUnicornTankState.Run:
					{
						RunRunningState(ref tank, ref boss, ref speed, ref animator);
						break;
					}
				}
			}
		}

		private static void RunRunningState(
			ref BossUnicornTankStateComponent tank,
			ref BossUnicornComponent boss,
			ref SpeedComponent speed,
			ref AnimatorComponent animator)
		{
			speed.InitialSpeed = boss.TankSpeed;
			
			animator.Animator.SetBool(s_Attack, true);

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.State = BossUnicornTankState.Stop;
			tank.TimeLeft = boss.TankStoppingDuration;
		}

		private static void RunStoppingState(
			ref BossUnicornComponent boss, 
			ref BossUnicornTankStateComponent tank,
			ref SpeedComponent speed,
			ref AnimatorComponent animator,
			EcsPool<BossUnicornTankStateComponent> tankStatePool,
			int entityId,
			EcsPool<BossUnicornFollowStateComponent> followStatePool)
		{
			speed.InitialSpeed = 0;
			
			animator.Animator.SetBool(s_Attack, false);

			if (tank.TimeLeft > 0)
			{
				return;
			}

			tank.Counts++;

			if (tank.Counts < boss.TankCount)
			{
				tank.State = BossUnicornTankState.Prepare;
				tank.TimeLeft = boss.TankPreparingDuration;
			}

			tankStatePool.Del(entityId);
			followStatePool.Add(entityId);
		}

		private static void RunPreparingState(
			EcsPool<PositionComponent> positionPool,
			int entityId,
			EcsPool<MovementComponent> movementPool,
			Vector2 playerPosition,
			ref BossUnicornTankStateComponent tank,
			ref BossUnicornComponent boss,
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

			tank.State = BossUnicornTankState.Run;
			tank.TimeLeft = boss.TankRunningDuration;
		}
	}
}