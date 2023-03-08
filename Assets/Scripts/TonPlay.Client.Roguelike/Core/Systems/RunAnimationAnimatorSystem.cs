using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RunAnimationAnimatorSystem : IEcsRunSystem
	{
		private static readonly int s_Run = Animator.StringToHash("Run");
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<AnimatorComponent>()
						.Inc<MovementComponent>()
						.Exc<DeadComponent>()
						.Exc<DestroyComponent>()
						.End();

			var animatorPool = world.GetPool<AnimatorComponent>();
			var movementPool = world.GetPool<MovementComponent>();

			foreach (var entityIdx in filter)
			{
				ref var animator = ref animatorPool.Get(entityIdx);
				ref var movement = ref movementPool.Get(entityIdx);

				animator.Animator.SetFloat(s_Run, movement.Direction.sqrMagnitude <= 0.01 ? 0 : 1);
			}
		}
	}
}