using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class DestroyEntitySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
 #region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyComponent>().End();

			foreach (var entityId in filter)
			{
				world.DelEntity(entityId);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}