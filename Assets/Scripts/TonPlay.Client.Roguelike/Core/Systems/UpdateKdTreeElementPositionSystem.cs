using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class UpdateKdTreeElementPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);

			var world = systems.GetWorld();
			var filter = world
						.Filter<KdTreeElementComponent>()
						.Inc<PositionComponent>()
						.Exc<DestroyComponent>()
						.Exc<InactiveComponent>()
						.Exc<DeadComponent>()
						.End();

			var kdTreeElementPool = world.GetPool<KdTreeElementComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var kdTreeElement = ref kdTreeElementPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				kdTreeElement.Storage.UpdateElement(entityId, position.Position);
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}