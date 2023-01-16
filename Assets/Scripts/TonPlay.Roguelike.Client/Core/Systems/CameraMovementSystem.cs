using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class CameraMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
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
				transformComponent.Transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 7);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}