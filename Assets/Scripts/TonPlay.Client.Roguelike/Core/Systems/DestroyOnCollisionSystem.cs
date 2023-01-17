using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class DestroyOnCollisionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>()
							  .Inc<DestroyOnCollisionComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			var pool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				pool.AddOrGet(entityId);
			}
		}
	}
}