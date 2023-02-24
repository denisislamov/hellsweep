using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearUsedEventsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<EventComponent>().Inc<UsedEventComponent>().End();

			foreach (var entityId in filter)
			{
				world.DelEntity(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}