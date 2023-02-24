using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using ModestTree;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine.Profiling;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CollisionSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private KDQuery _query = new KDQuery();

		public CollisionSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();

			var filter = world.Filter<HasCollidedComponent>()
							  .Inc<PositionComponent>()
							  .Inc<CollisionComponent>()
							  .Exc<InactiveComponent>()
							  .Exc<DoNotInitiateCollisionOverlap>()
							  .End();

			var collisionPool = world.GetPool<CollisionComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var hasCollidedPool = world.GetPool<HasCollidedComponent>();

			var overlapParams = OverlapParams.Create(world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());

			foreach (var entityId in filter)
			{
				ref var collision = ref collisionPool.Get(entityId);

				if (collision.CollisionAreaConfig.DoNotInitiateCollisionOverlap)
				{
					continue;
				}

				ref var hasCollided = ref hasCollidedPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				hasCollided.CollidedEntityIds.Clear();

				_overlapExecutor.Overlap(
					_query,
					position.Position,
					collision.CollisionAreaConfig,
					ref hasCollided.CollidedEntityIds,
					collision.LayerMask,
					overlapParams);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}