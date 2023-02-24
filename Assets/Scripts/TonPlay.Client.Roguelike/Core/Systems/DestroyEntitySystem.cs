using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyEntitySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyComponent>().End();

			foreach (var entityId in filter)
			{
				world.DelEntity(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}