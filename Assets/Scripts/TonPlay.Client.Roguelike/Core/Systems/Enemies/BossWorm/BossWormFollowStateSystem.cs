using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using UnityEngine;
using Time = UnityEngine.Time;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossWorm
{
	public class BossWormFollowStateSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossWormComponent>()
							  .Inc<BossWormFollowStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Exc<DeadComponent>()
							  .Exc<DestroyComponent>()
							  .End();

			var bossPool = world.GetPool<BossWormComponent>();
			var followStatePool = world.GetPool<BossWormFollowStateComponent>();
			var shootStatePool = world.GetPool<BossWormShootStateComponent>();
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
				ref var follow = ref followStatePool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);

				if (!follow.IsInited)
				{
					follow.IsInited = true;
					follow.TimeLeft = boss.FollowStateDuration;
				}

				follow.TimeLeft -= Time.deltaTime;

				if (follow.TimeLeft <= 0)
				{
					followStatePool.Del(entityId);

					var random = Random.Range(0f, 1f);
					if (random <= 0.5f)
					{
						shootStatePool.Add(entityId);
					}
					else
					{
						tankStatePool.Add(entityId);
					}

					speed.Speed = 0f;
					continue;
				}

				var position = positionPool.Get(entityId).Position;
				ref var movement = ref movementPool.Get(entityId);

				movement.Direction = playerPosition - position;
				movement.Direction.Normalize();
				
				speed.Speed = boss.FollowSpeed;
			}
		}
	}
}