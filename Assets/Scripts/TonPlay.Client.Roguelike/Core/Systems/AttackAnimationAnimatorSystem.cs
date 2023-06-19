using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class AttackAnimationAnimatorSystem : IEcsRunSystem
	{
		private static readonly int s_Attack = Animator.StringToHash("Attack");

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();

			var animatorPool = world.GetPool<AnimatorComponent>();
			var animsPool = world.GetPool<AnimationsComponent>();
			var attackAnimPool = world.GetPool<AttackAnimationComponent>();

			PlayAnimations(world, animatorPool, attackAnimPool);
			AddAnimationsOnAttack(world, animsPool, attackAnimPool);
		}

		private static void AddAnimationsOnAttack(
			EcsWorld world,
			EcsPool<AnimationsComponent> animsPool,
			EcsPool<AttackAnimationComponent> attackAnimPool)
		{
			var filter = world
						.Filter<AnimatorComponent>()
						.Inc<AnimationsComponent>()
						.Inc<AttackEvent>()
						.Inc<VisibleComponent>()
						.Exc<AttackAnimationComponent>()
						.Exc<DeadComponent>()
						.Exc<DestroyComponent>()
						.End();

			foreach (var entityIdx in filter)
			{
				var anims = animsPool.Get(entityIdx);
				ref var attackAnim = ref attackAnimPool.AddOrGet(entityIdx);

				attackAnim.TimeLeft = anims.AttackDuration;
			}
		}
		
		private static void PlayAnimations(EcsWorld world, EcsPool<AnimatorComponent> animatorPool, EcsPool<AttackAnimationComponent> attackAnimPool)
		{
			var filter = world
						.Filter<AnimatorComponent>()
						.Inc<AttackAnimationComponent>()
						.Inc<VisibleComponent>()
						.Exc<DeadComponent>()
						.Exc<DestroyComponent>()
						.End();

			foreach (var entityIdx in filter)
			{
				ref var animator = ref animatorPool.Get(entityIdx);
				ref var attackAnim = ref attackAnimPool.Get(entityIdx);

				animator.Animator.SetFloat(s_Attack, 1);

				attackAnim.TimeLeft -= Time.deltaTime;

				if (attackAnim.TimeLeft < 0)
				{
					attackAnimPool.Del(entityIdx);
					
					animator.Animator.SetFloat(s_Attack, 0);
				}
			}
		}
	}
}