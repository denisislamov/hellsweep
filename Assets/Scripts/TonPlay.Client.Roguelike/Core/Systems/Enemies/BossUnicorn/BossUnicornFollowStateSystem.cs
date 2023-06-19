using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossUnicorn;
using UnityEngine;
using Time = UnityEngine.Time;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.BossUnicorn
{
	public class BossUnicornFollowStateSystem : IEcsRunSystem
	{
		private static readonly int s_Attack = Animator.StringToHash("Attack");
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<BossUnicornComponent>()
							  .Inc<BossUnicornFollowStateComponent>()
							  .Inc<MovementComponent>()
							  .Inc<SpeedComponent>()
							  .Inc<DamageOnCollisionComponent>()
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
				ref var follow = ref followStatePool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);
				ref var animator = ref animatorPool.Get(entityId);

				if (!follow.IsInited)
				{
					ref var damage = ref damagePool.Get(entityId);

					follow.IsInited = true;
					follow.TimeLeft = boss.FollowStateDuration;
					damage.DamageProvider = boss.FollowDamageProvider;
				}
				
				animator.Animator.SetBool(s_Attack, false);

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