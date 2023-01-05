using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class CameraMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var playerFilter = world.Filter<RigidbodyComponent>().Inc<PlayerComponent>().End();
			var cameraFilter = world.Filter<CameraComponent>().Inc<TransformComponent>().End();
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();
			var transformComponents = world.GetPool<TransformComponent>();
			var playerPosition = Vector3.zero;

			foreach (var entityId in playerFilter)
			{
				ref var playerMovementComponent = ref rigidbodyComponents.Get(entityId);
				playerPosition = playerMovementComponent.Rigidbody.position;
			}

			foreach (var entityId in cameraFilter)
			{
				ref var transformComponent = ref transformComponents.Get(entityId);
				var currentPosition = transformComponent.Transform.position;
				var targetPosition = new Vector3(playerPosition.x, playerPosition.y, currentPosition.z);
				transformComponent.Transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 7);
			}
		}
	}
}