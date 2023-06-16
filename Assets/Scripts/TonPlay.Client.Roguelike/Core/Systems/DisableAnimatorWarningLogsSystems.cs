using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DisableAnimatorWarningLogsSystems : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<AnimatorComponent>().Exc<DisableWarningLogComponent>().End();
				var disablePool = world.GetPool<DisableWarningLogComponent>();
				var animatorPool = world.GetPool<AnimatorComponent>();
				
				foreach (var entityIdx in filter)
				{
					ref var animator = ref animatorPool.Get(entityIdx);
					animator.Animator.logWarnings = false;
					disablePool.Add(entityIdx);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}