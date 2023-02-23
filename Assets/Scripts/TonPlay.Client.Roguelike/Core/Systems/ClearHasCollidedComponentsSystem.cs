using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearHasCollidedComponentsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>().End();
			var pool = world.GetPool<HasCollidedComponent>();

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref pool.Get(entityId);
				hasCollided.CollidedEntityIds.Clear();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}