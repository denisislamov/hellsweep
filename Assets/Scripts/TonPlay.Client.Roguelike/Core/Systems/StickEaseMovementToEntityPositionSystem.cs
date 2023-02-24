using DG.Tweening;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class StickEaseMovementToEntityPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<EaseMovementComponent>()
						.Inc<StickEaseMovementToEntityPositionComponent>()
						.Exc<DestroyComponent>()
						.End();

			var stickPool = world.GetPool<StickEaseMovementToEntityPositionComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var easeMovementPool = world.GetPool<EaseMovementComponent>();

			foreach (var entityId in filter)
			{
				ref var stick = ref stickPool.Get(entityId);
				ref var easeMovement = ref easeMovementPool.Get(entityId);

				if (!positionPool.Has(stick.EntityId))
				{
					continue;
				}

				ref var position = ref positionPool.Get(stick.EntityId);

				easeMovement.ToPosition = position.Position;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}