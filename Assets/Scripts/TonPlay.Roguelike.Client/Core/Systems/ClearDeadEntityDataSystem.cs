using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ClearDeadEntityDataSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			var filter = world.Filter<DeadComponent>()
							  .Inc<PositionComponent>()
							  .End();

			var damageOnCollisionPool = world.GetPool<DamageOnCollisionComponent>();
			var collisionPool = world.GetPool<CollisionComponent>();
			
			foreach (var entityId in filter)
			{
				if (damageOnCollisionPool.Has(entityId))
				{
					damageOnCollisionPool.Del(entityId);
				}

				if (collisionPool.Has(entityId))
				{
					collisionPool.Del(entityId);
				}
			}
		}
	}
}