using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ProjectileExplodeOnMoveDistanceSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;
		private readonly int _layerMask;

		private KDQuery _query = new KDQuery();
		private List<int> _overlappedEntities = new List<int>();

		public ProjectileExplodeOnMoveDistanceSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
			_layerMask = LayerMask.GetMask("Enemy");
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world
						.Filter<ProjectileComponent>()
						.Inc<ExplodeOnMoveDistanceComponent>()
						.Inc<PositionComponent>()
						.Exc<DestroyComponent>()
						.Exc<InactiveComponent>()
						.End();

			var destroyPool = world.GetPool<DestroyComponent>();
			var explodePool = world.GetPool<ExplodeOnMoveDistanceComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var applyDamagePool = world.GetPool<ApplyDamageComponent>();

			foreach (var entityId in filter)
			{
				ref var explodeOnMove = ref explodePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (Vector2.Distance(position.Position, explodeOnMove.StartPosition) >= explodeOnMove.DistanceToExplode)
				{
					var count = _overlapExecutor.Overlap(_query, position.Position, explodeOnMove.CollisionConfig, ref _overlappedEntities, _layerMask);
					for (var i = 0; i < count; i++)
					{
						var enemyEntityId = _overlappedEntities[i];
						if (applyDamagePool.Has(enemyEntityId))
						{
							ref var applyDamage = ref applyDamagePool.Get(enemyEntityId);
							applyDamage.Damage += explodeOnMove.Damage;
						}
						else
						{
							ref var applyDamage = ref applyDamagePool.Add(enemyEntityId);
							applyDamage.Damage += explodeOnMove.Damage;
						}
					}
					
					_overlappedEntities.Clear();

					destroyPool.Add(entityId);
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}