using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ClearDeadEntityDataSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
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
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}