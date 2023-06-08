using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeWarrior;
using UnityEngine;
using Time = UnityEngine.Time;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossJadeWarrior
{
	public class BossJadeWarriorFollowStateSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossJadeWarriorComponent>()
							  .Inc<BossJadeWarriorFollowStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Inc<DamageOnCollisionComponent>()
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
					ref var damage = ref damagePool.Get(entityId);

					follow.IsInited = true;
					follow.TimeLeft = boss.FollowStateDuration;
					damage.DamageProvider = boss.FollowDamageProvider;
				}

				follow.TimeLeft -= Time.deltaTime;

				if (follow.TimeLeft <= 0)
				{
					speed.InitialSpeed = 0f;

					followStatePool.Del(entityId);
					tankStatePool.Add(entityId);

					continue;
				}

				var position = positionPool.Get(entityId).Position;
				ref var movement = ref movementPool.Get(entityId);

				movement.Direction = playerPosition - position;
				movement.Direction.Normalize();
				
				speed.InitialSpeed = boss.FollowSpeed;
			}
		}
	}
}