using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearHasCollidedComponentsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>().End();
			var pool = world.GetPool<HasCollidedComponent>();

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref pool.Get(entityId);
				hasCollided.CollidedEntityIds.Clear();
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}