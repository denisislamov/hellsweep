using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ToggleMonoAnimatorOnVisibilityChangedSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld[] _worlds;

		public void Init(EcsSystems systems)
		{
			_worlds = new EcsWorld[]
			{
				systems.GetWorld(),
				systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
			};
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			for (var index = 0; index < _worlds.Length; index++)
			{
				var world = _worlds[index];
				var animatorPool = world.GetPool<AnimatorComponent>();
				var enablePool = world.GetPool<MonoAnimatorEnabledComponent>();

				FilterAndEnableMonoComponents(world, animatorPool, enablePool);
				FilterAndDisableMonoComponents(world, animatorPool, enablePool);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private static void FilterAndDisableMonoComponents(EcsWorld world, EcsPool<AnimatorComponent> animatorPool, EcsPool<MonoAnimatorEnabledComponent> enablePool)
		{
			var filterToDisable = world.Filter<AnimatorComponent>().Inc<MonoAnimatorEnabledComponent>().Exc<VisibleComponent>().End();
			
			foreach (var entityIdx in filterToDisable)
			{
				ref var animatorComponent = ref animatorPool.Get(entityIdx);

				if (animatorComponent.Animator != null)
				{
					animatorComponent.Animator.enabled = false;

					enablePool.Del(entityIdx);
				}
			}
		}
		
		private static void FilterAndEnableMonoComponents(EcsWorld world, EcsPool<AnimatorComponent> animatorPool, EcsPool<MonoAnimatorEnabledComponent> enablePool)
		{
			var filterToEnable = world.Filter<AnimatorComponent>().Inc<VisibleComponent>().Exc<MonoAnimatorEnabledComponent>().End();

			foreach (var entityIdx in filterToEnable)
			{
				ref var animatorComponent = ref animatorPool.Get(entityIdx);

				if (animatorComponent.Animator != null)
				{
					animatorComponent.Animator.enabled = true;

					enablePool.Add(entityIdx);
				}
			}
		}
	}
}