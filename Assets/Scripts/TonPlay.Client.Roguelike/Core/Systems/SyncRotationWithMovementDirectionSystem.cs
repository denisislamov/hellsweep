using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncRotationWithMovementDirectionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<SyncRotationWithMovementDirectionComponent>().Inc<RotationComponent>().Inc<MovementComponent>().End();
			var rotationPool = world.GetPool<RotationComponent>();
			var movementPool = world.GetPool<MovementComponent>();

			foreach (var entityId in filter)
			{
				ref var rotation = ref rotationPool.Get(entityId);
				ref var movement = ref movementPool.Get(entityId);

				rotation.Direction = movement.Direction;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}