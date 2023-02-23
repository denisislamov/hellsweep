using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DamageOnDistanceChangeSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private KDQuery _query = new KDQuery();
		private List<int> _overlappedEntities = new List<int>();

		public DamageOnDistanceChangeSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<DamageOnDistanceChangeComponent>()
						.Inc<PositionComponent>()
						.Inc<CollisionComponent>()
						.Exc<DestroyComponent>()
						.Exc<InactiveComponent>()
						.End();

			var damageOnDistancePool = world.GetPool<DamageOnDistanceChangeComponent>();
			var blockDamagePerRatePool = world.GetPool<DamageOnDistanceChangeComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var collisionPool = world.GetPool<CollisionComponent>();
			var stackTryApplyDamagePool = world.GetPool<StackTryApplyDamageComponent>();

			var overlapParams = OverlapParams.Create(world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());
			overlapParams.Build();

			foreach (var entityId in filter)
			{
				ref var damageOnDistance = ref damageOnDistancePool.Get(entityId);
				ref var collision = ref collisionPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				var collisionArea = (ICircleCollisionAreaConfig)collision.CollisionAreaConfig;

				if (Vector2.Distance(damageOnDistance.LastDamagePosition, position.Position) < collisionArea.Radius)
				{
					continue;
				}

				damageOnDistance.LastDamagePosition = position.Position;

				var count = _overlapExecutor.Overlap(
					_query,
					position.Position,
					collision.CollisionAreaConfig,
					ref _overlappedEntities,
					collision.LayerMask,
					overlapParams);

				for (int i = 0; i < count; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];

					ref var stack = ref stackTryApplyDamagePool.Get(entityId);
					stack.Stack.Push(new TryApplyDamageComponent()
					{
						DamageProvider = damageOnDistance.DamageProvider,
						VictimEntityId = overlappedEntityId,
					});
				}

				_overlappedEntities.Clear();
			}
		}
	}
}