using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RunBloodAnimationAnimatorSystem : IEcsRunSystem
	{
		private static readonly int s_Active = Animator.StringToHash("Active");
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<BloodAnimatorComponent>()
						.Inc<ApplyDamageComponent>()
						.Exc<DeadComponent>()
						.Exc<DestroyComponent>()
						.End();

			var animatorPool = world.GetPool<BloodAnimatorComponent>();

			foreach (var entityIdx in filter)
			{
				ref var animator = ref animatorPool.Get(entityIdx);

				animator.Animator.SetBool(s_Active, true);
			}
			
			filter = world
					.Filter<BloodAnimatorComponent>()
					.Exc<ApplyDamageComponent>()
					.Exc<DeadComponent>()
					.Exc<DestroyComponent>()
					.End();
			
			foreach (var entityIdx in filter)
			{
				ref var animator = ref animatorPool.Get(entityIdx);

				animator.Animator.SetBool(s_Active, false);
			}
		}
	}
}