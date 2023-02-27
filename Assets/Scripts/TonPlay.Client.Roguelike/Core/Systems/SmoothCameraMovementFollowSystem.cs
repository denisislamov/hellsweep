using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SmoothCameraMovementFollowSystem : IEcsRunSystem
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
							  .Exc<CameraShakeComponent>()
							  .End();

			var positionPool = world.GetPool<PositionComponent>();
			var transformPool = world.GetPool<TransformComponent>();

			foreach (var entityId in filter)
			{
				var playerPosition = positionPool.Get(playerEntityId).Position;

				ref var transformComponent = ref transformPool.Get(entityId);
				var currentPosition = transformComponent.Transform.position;
				var targetPosition = new Vector3(playerPosition.x, playerPosition.y, currentPosition.z);
				transformComponent.Transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime*7);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}