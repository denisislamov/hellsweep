using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Locations.Sands;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Locations.Sands
{
	public class SandStormCameraShakeSystem : IEcsRunSystem
	{
		private const float REACTION_DISTANCE = 25f;
		private const float MAX_SHAKE_MAGNITUDE_DISTANCE = 5f;
		private const float MAX_SHAKE_MAGNITUDE = 0.175f;

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var stormFilter = world.Filter<SandStormComponent>().Inc<PositionComponent>().Exc<DestroyComponent>().End();
			var cameraFilter = world.Filter<CameraComponent>().Inc<TransformComponent>().Exc<DestroyComponent>().End();
			
			var shakePool = world.GetPool<CameraShakeComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var transformPool = world.GetPool<TransformComponent>();

			foreach (var stormEntityIdx in stormFilter)
			{
				ref var stormPosition = ref positionPool.Get(stormEntityIdx);
				
				foreach (var cameraEntityIdx in cameraFilter)
				{
					ref var cameraTransform = ref transformPool.Get(cameraEntityIdx);

					var distance = Vector2.Distance(cameraTransform.Transform.position, stormPosition.Position);
					if (distance > REACTION_DISTANCE)
					{
						continue;
					}

					ref var cameraShake = ref shakePool.AddOrGet(cameraEntityIdx);

					var unclampedShakeScale = (REACTION_DISTANCE - distance) / (REACTION_DISTANCE - MAX_SHAKE_MAGNITUDE_DISTANCE);
					var clampedShakeScale = unclampedShakeScale > 1f ? 1f : unclampedShakeScale;
					var shakeMagnitude = MAX_SHAKE_MAGNITUDE * clampedShakeScale;

					if (cameraShake.ShakeMagnitude > shakeMagnitude)
					{
						continue;
					}
					
					cameraShake.ShakeMagnitude = shakeMagnitude;
					cameraShake.TimeLeft = RoguelikeConstants.Core.Camera.CAMERA_SHAKE_TIME;
				}
			}
		}
	}
}