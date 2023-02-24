using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CameraMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var playerFilter = world.Filter<PositionComponent>().Inc<PlayerComponent>().End();
			var cameraFilter = world.Filter<CameraComponent>().Inc<TransformComponent>().End();
			var positionPool = world.GetPool<PositionComponent>();
			var transformPool = world.GetPool<TransformComponent>();
			var playerPosition = Vector3.zero;

			foreach (var entityId in playerFilter)
			{
				ref var position = ref positionPool.Get(entityId);
				playerPosition = position.Position;
			}

			foreach (var entityId in cameraFilter)
			{
				ref var transformComponent = ref transformPool.Get(entityId);
				var currentPosition = transformComponent.Transform.position;
				var targetPosition = new Vector3(playerPosition.x, playerPosition.y, currentPosition.z);
				transformComponent.Transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime*7);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}