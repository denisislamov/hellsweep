using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EvaluatePlayableDirector : IEcsRunSystem, IEcsInitSystem
	{
		private const int DISTANCE_TO_CAMERA_TO_EVALUATE = 14;
		private const int SQR_DISTANCE_TO_CAMERA_TO_EVALUATE = DISTANCE_TO_CAMERA_TO_EVALUATE*DISTANCE_TO_CAMERA_TO_EVALUATE;
		
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
			
			var cameraFilter = systems.GetWorld().Filter<CameraComponent>().Inc<TransformComponent>().End();
			var mainWorldTransformPool = systems.GetWorld().GetPool<TransformComponent>();

			for (var index = 0; index < _worlds.Length; index++)
			{
				var world = _worlds[index];
				var filter = world.Filter<PlayableDirectorComponent>().Inc<PositionComponent>().End();
				var playablePool = world.GetPool<PlayableDirectorComponent>();
				var positionPool = world.GetPool<PositionComponent>();

				foreach (var cameraIdx in cameraFilter)
				{
					ref var cameraTransform = ref mainWorldTransformPool.Get(cameraIdx);

					foreach (var entityIdx in filter)
					{
						ref var playablePosition = ref positionPool.Get(entityIdx);

						if ((cameraTransform.Transform.position.ToVector2XY() - playablePosition.Position).SqrMagnitude() < SQR_DISTANCE_TO_CAMERA_TO_EVALUATE)
						{
							ref var playable = ref playablePool.Get(entityIdx);

							if (playable.PlayableDirector.playableGraph.IsValid())
							{
								playable.PlayableDirector.playableGraph.Evaluate(UnityEngine.Time.deltaTime);
							}
						}
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}