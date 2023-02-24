using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncRotationWithMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<SyncRotationWithPositionDifferenceComponent>().Inc<RotationComponent>().Inc<PositionComponent>().End();
			var rotationPool = world.GetPool<RotationComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var syncRotationWithPositionPool = world.GetPool<SyncRotationWithPositionDifferenceComponent>();

			foreach (var entityId in filter)
			{
				ref var rotation = ref rotationPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var syncRotationWithPosition = ref syncRotationWithPositionPool.Get(entityId);

				rotation.Direction = (position.Position - syncRotationWithPosition.LastPosition);
				rotation.Direction.Normalize();

				syncRotationWithPosition.LastPosition = Vector2.Lerp(syncRotationWithPosition.LastPosition, position.Position, Time.deltaTime*5);
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}