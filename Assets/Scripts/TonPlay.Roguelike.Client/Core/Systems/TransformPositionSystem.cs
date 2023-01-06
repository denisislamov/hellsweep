using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class TransformPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var transformComponents = world.GetPool<TransformComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var filter = world.Filter<TransformComponent>().Exc<CameraComponent>().End();

			foreach (var entityId in filter)
			{
				ref var transformComponent = ref transformComponents.Get(entityId);
				
				if (!positionComponents.Has(entityId))
				{
					ref var positionComponent = ref positionComponents.Add(entityId);
					positionComponent.Position = transformComponent.Transform.position;
				}
				else
				{
					ref var positionComponent = ref positionComponents.Get(entityId);
					transformComponent.Transform.position = positionComponent.Position;
				}
			}
		}
	}
}