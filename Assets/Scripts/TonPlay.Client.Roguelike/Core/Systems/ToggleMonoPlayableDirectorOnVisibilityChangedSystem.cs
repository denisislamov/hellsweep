using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Animator;
using TonPlay.Client.Roguelike.Core.Components.PlayableDirector;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ToggleMonoPlayableDirectorOnVisibilityChangedSystem : IEcsRunSystem, IEcsInitSystem
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
				var animatorPool = world.GetPool<PlayableDirectorComponent>();
				var enablePool = world.GetPool<MonoPlayableDirectorEnabledComponent>();

				FilterAndEnableMonoComponents(world, animatorPool, enablePool);
				FilterAndDisableMonoComponents(world, animatorPool, enablePool);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private static void FilterAndDisableMonoComponents(EcsWorld world, EcsPool<PlayableDirectorComponent> animatorPool, EcsPool<MonoPlayableDirectorEnabledComponent> enablePool)
		{
			var filterToDisable = world.Filter<PlayableDirectorComponent>().Inc<MonoPlayableDirectorEnabledComponent>().Exc<VisibleComponent>().End();
			
			foreach (var entityIdx in filterToDisable)
			{
				ref var playableDirectorComponent = ref animatorPool.Get(entityIdx);

				if (playableDirectorComponent.PlayableDirector != null)
				{
					playableDirectorComponent.PlayableDirector.enabled = false;

					enablePool.Del(entityIdx);
				}
			}
		}
		
		private static void FilterAndEnableMonoComponents(EcsWorld world, EcsPool<PlayableDirectorComponent> animatorPool, EcsPool<MonoPlayableDirectorEnabledComponent> enablePool)
		{
			var filterToEnable = world.Filter<PlayableDirectorComponent>().Inc<VisibleComponent>().Exc<MonoPlayableDirectorEnabledComponent>().End();

			foreach (var entityIdx in filterToEnable)
			{
				ref var playableDirectorComponent = ref animatorPool.Get(entityIdx);

				if (playableDirectorComponent.PlayableDirector != null)
				{
					playableDirectorComponent.PlayableDirector.enabled = true;

					enablePool.Add(entityIdx);
				}
			}
		}
	}
}