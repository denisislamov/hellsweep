using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.PlayableDirector;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EvaluatePlayableDirectorSystem : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<PlayableDirectorComponent>().Inc<VisibleComponent>().End();
				var playablePool = world.GetPool<PlayableDirectorComponent>();

				foreach (var entityIdx in filter)
				{
					ref var playable = ref playablePool.Get(entityIdx);

					if (playable.PlayableDirector.playableGraph.IsValid())
					{
						playable.PlayableDirector.playableGraph.Evaluate(UnityEngine.Time.deltaTime);
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}