using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ClearUsedEventsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<EventComponent>().Inc<UsedEventComponent>().End();

			foreach (var entityId in filter)
			{
				world.DelEntity(entityId);
			}
		}
	}
}