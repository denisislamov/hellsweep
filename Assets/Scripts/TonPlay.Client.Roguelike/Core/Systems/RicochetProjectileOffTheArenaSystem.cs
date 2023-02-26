using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RicochetProjectileOffTheArenaSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;
		private readonly KDQuery _kdQuery = new KDQuery();

		public RicochetProjectileOffTheArenaSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Run(EcsSystems systems)
		{
			// TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			//
			// var world = systems.GetWorld();
			// var arenaFilter = world
			// 				 .Filter<Arena>()
			// 				 .Exc<DestroyComponent>()
			// 				 .End();
			//
			// if (arenaFilter.GetEntitiesCount() <= 0)
			// {
			// 	return;
			// }
			//
			// var filter = world
			// 			.Filter<HasCollidedComponent>()
			// 			.Inc<ProjectileComponent>()
			// 			.Inc<RicochetOffTheArenaComponent>()
			// 			.Inc<MovementComponent>()
			// 			.Exc<DestroyComponent>()
			// 			.End();
			//
			// var hasCollidedPool = world.GetPool<HasCollidedComponent>();
			// var layerPool = world.GetPool<LayerComponent>();
			// var ricochetPool = world.GetPool<RicochetOffTheArenaComponent>();
			// var movementPool = world.GetPool<MovementComponent>();
			//
			// foreach (var entityId in filter)
			// {
			// 	ref var hasCollided = ref hasCollidedPool.Get(entityId);
			// 	
			// 	if (hasCollided.CollidedEntityIds.Count <= 0) continue;
			// 	
			// 	ref var ricochet = ref ricochetPool.Get(entityId);
			//
			// 	for (var i = 0; i < hasCollided.CollidedEntityIds.Count; i++)
			// 	{
			// 		var collidedEntityId = hasCollided.CollidedEntityIds[i];
			//
			// 		ref var collidedEntityLayer = ref layerPool.Get(collidedEntityId);
			//
			// 		if (LayerMaskExt.ContainsLayer(ricochet.LayerMask, collidedEntityLayer.Layer))
			// 		{
			// 			ref var collidedEntityMovement = ref movementPool.Get(collidedEntityId);
			// 			
			// 			collidedEntityMovement.Direction
			// 		}
			// 	}
			// }
			//
			// TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}