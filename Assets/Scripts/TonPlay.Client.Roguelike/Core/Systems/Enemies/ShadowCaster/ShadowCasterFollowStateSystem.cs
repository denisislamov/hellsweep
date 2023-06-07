using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using TonPlay.Client.Roguelike.Core.Components.Enemies.ShadowCasterMiniboss;
using UnityEngine;
using Time = UnityEngine.Time;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.ShadowCaster
{
	public class ShadowCasterFollowStateSystem : IEcsRunSystem
	{
		private static readonly int s_Shoot = Animator.StringToHash("Shoot");
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<ShadowCasterComponent>()
							  .Inc<ShadowCasterFollowStateComponent>()
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
			var movementPool = world.GetPool<MovementComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();
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
				ref var animator = ref animatorPool.Get(entityId);
				
				animator.Animator.SetBool(s_Shoot, false);
				
				if (!follow.IsInited)
				{
					follow.IsInited = true;
					follow.TimeLeft = boss.FollowStateDuration;
				}

				follow.TimeLeft -= Time.deltaTime;

				if (follow.TimeLeft <= 0)
				{
					followStatePool.Del(entityId);

					shootStatePool.Add(entityId);

					speed.InitialSpeed = 0f;
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