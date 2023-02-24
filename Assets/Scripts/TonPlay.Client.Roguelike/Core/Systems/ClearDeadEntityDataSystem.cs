using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearDeadEntityDataSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
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
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}