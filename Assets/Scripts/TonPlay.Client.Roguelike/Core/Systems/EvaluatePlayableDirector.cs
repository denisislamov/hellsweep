using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EvaluatePlayableDirector : IEcsRunSystem
	{
		private const int DISTANCE_TO_CAMERA_TO_EVALUATE = 14;
		private const int SQR_DISTANCE_TO_CAMERA_TO_EVALUATE = DISTANCE_TO_CAMERA_TO_EVALUATE*DISTANCE_TO_CAMERA_TO_EVALUATE;
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<PlayableDirectorComponent>().Inc<PositionComponent>().End();
			var cameraFilter = world.Filter<CameraComponent>().Inc<TransformComponent>().End();
			var playablePool = world.GetPool<PlayableDirectorComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var transformPool = world.GetPool<TransformComponent>();

			foreach (var cameraIdx in cameraFilter)
			{
				ref var cameraTransform = ref transformPool.Get(cameraIdx);
				
				foreach (var entityIdx in filter)
				{
					ref var playablePosition = ref positionPool.Get(entityIdx);

					if ((cameraTransform.Transform.position.ToVector2XY() - playablePosition.Position).SqrMagnitude() < SQR_DISTANCE_TO_CAMERA_TO_EVALUATE)
					{
						ref var playable = ref playablePool.Get(entityIdx);
						playable.PlayableDirector.playableGraph.Evaluate(UnityEngine.Time.deltaTime);
					}
				}
			}
		}
	}
}