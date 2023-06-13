using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CameraShakeAndFollowSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var playerFilter = world.Filter<PositionComponent>().Inc<PlayerComponent>().End();

			var playerEntityId = EcsEntity.DEFAULT_ID;
			foreach (var entityId in playerFilter)
			{
				playerEntityId = entityId;
				break;
			}

			var filter = world.Filter<CameraComponent>()
							  .Inc<TransformComponent>()
							  .Inc<CameraShakeComponent>()
							  .End();

			var positionPool = world.GetPool<PositionComponent>();
			var transformPool = world.GetPool<TransformComponent>();
			var shakePool = world.GetPool<CameraShakeComponent>();

			foreach (var entityId in filter)
			{
				var playerPosition = positionPool.Get(playerEntityId).Position;

				ref var shake = ref shakePool.Get(entityId);
				ref var transformComponent = ref transformPool.Get(entityId);
				
				var currentPosition = transformComponent.Transform.position;
				var targetPosition = new Vector3(playerPosition.x, playerPosition.y, currentPosition.z);

				if (shake.TimeLeft <= 0)
				{
					transformComponent.Transform.position = targetPosition;
					shakePool.Del(entityId);
					continue;
				}

				shake.TimeLeft -= Time.deltaTime;
				
				transformComponent.Transform.position = 
					targetPosition + 
					shake.ShakeMagnitude * shake.TimeLeft / RoguelikeConstants.Core.Camera.CAMERA_SHAKE_TIME * Random.insideUnitSphere;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}