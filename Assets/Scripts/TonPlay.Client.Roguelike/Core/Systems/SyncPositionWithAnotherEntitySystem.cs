using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncPositionWithAnotherEntitySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();

			var filter = world.Filter<SyncPositionWithAnotherEntityComponent>().Inc<PositionComponent>().Exc<InactiveComponent>().End();

			var syncPool = world.GetPool<SyncPositionWithAnotherEntityComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var sync = ref syncPool.Get(entityId);

				if (positionPool.Has(sync.ParentEntityId))
				{
					ref var position = ref positionPool.Get(entityId);
					ref var parentPosition = ref positionPool.Get(sync.ParentEntityId);
					position.Position = parentPosition.Position;
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}