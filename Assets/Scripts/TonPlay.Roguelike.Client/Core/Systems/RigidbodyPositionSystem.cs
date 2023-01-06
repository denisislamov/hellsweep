using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class RigidbodyPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var filter = world.Filter<RigidbodyComponent>().End();

			foreach (var entityId in filter)
			{
				ref var transformComponent = ref rigidbodyComponents.Get(entityId);
				
				if (positionComponents.Has(entityId))
				{
					ref var positionComponent = ref positionComponents.Get(entityId);
					transformComponent.Rigidbody.MovePosition(positionComponent.Position);
				}
				else
				{
					ref var positionComponent = ref positionComponents.Add(entityId);
					positionComponent.Position = transformComponent.Rigidbody.position;
				}
			}
		}
	}
}