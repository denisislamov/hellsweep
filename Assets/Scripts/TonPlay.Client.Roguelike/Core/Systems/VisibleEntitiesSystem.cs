using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class VisibleEntitiesSystem : IEcsRunSystem, IEcsInitSystem
	{
		private const int DISTANCE_TO_CAMERA_TO_ASSUME_VISIBILITY = 15;
		private const int SQR_DISTANCE_TO_CAMERA = DISTANCE_TO_CAMERA_TO_ASSUME_VISIBILITY*DISTANCE_TO_CAMERA_TO_ASSUME_VISIBILITY;
		
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
				var notVisibleFilter = world.Filter<PositionComponent>().Exc<VisibleComponent>().End();
				var positionPool = world.GetPool<PositionComponent>();
				var visiblePool = world.GetPool<VisibleComponent>();

				var visibleFilter = world.Filter<PositionComponent>().Inc<VisibleComponent>().End();
				foreach (var cameraIdx in cameraFilter)
				{
					ref var cameraTransform = ref mainWorldTransformPool.Get(cameraIdx);

					TryToDisableVisibleEntities(visibleFilter, positionPool, cameraTransform, visiblePool);
					TryToEnableInvisibleEntities(notVisibleFilter, positionPool, cameraTransform, visiblePool);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private static void TryToEnableInvisibleEntities(
			EcsFilter notVisibleFilter, 
			EcsPool<PositionComponent> positionPool, 
			TransformComponent cameraTransform, 
			EcsPool<VisibleComponent> visiblePool)
		{
			foreach (var entityIdx in notVisibleFilter)
			{
				ref var position = ref positionPool.Get(entityIdx);

				if (ShouldBeActive(cameraTransform, position))
				{
					visiblePool.Add(entityIdx);
				}
			}
		}
		
		private static void TryToDisableVisibleEntities(
			EcsFilter visibleFilter, 
			EcsPool<PositionComponent> positionPool, 
			TransformComponent cameraTransform, 
			IEcsPool visiblePool)
		{
			foreach (var entityIdx in visibleFilter)
			{
				ref var position = ref positionPool.Get(entityIdx);

				if (!ShouldBeActive(cameraTransform, position))
				{
					visiblePool.Del(entityIdx);
				}
			}
		}

		private static bool ShouldBeActive(TransformComponent cameraTransform, PositionComponent positionComponent)
		{
			return (cameraTransform.Transform.position.ToVector2XY() - positionComponent.Position).SqrMagnitude() < SQR_DISTANCE_TO_CAMERA;
		}
	}
}