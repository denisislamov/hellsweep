using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ExplosionSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;
		private readonly KDQuery _query = new KDQuery();
		
		private List<int> _overlappedEntities = new List<int>();

		public ExplosionSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();

			var filter = world.Filter<ExplosionComponent>().Inc<PositionComponent>().End();
			var positionPool = world.GetPool<PositionComponent>();
			var explosionPool = world.GetPool<ExplosionComponent>();
			var applyDamagePool = world.GetPool<ApplyDamageComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var explosion = ref explosionPool.Get(entityId);
				
				var count = _overlapExecutor.Overlap(_query, position.Position, explosion.CollisionAreaConfig, ref _overlappedEntities, explosion.LayerMask);
				
				for (var i = 0; i < count; i++)
				{
					var enemyEntityId = _overlappedEntities[i];
					if (applyDamagePool.Has(enemyEntityId))
					{
						ref var applyDamage = ref applyDamagePool.Get(enemyEntityId);
						applyDamage.Damage += explosion.Damage;
					}
					else
					{
						ref var applyDamage = ref applyDamagePool.Add(enemyEntityId);
						applyDamage.Damage += explosion.Damage;
					}
				}
				
				_overlappedEntities.Clear();

				world.DelEntity(entityId);
			}
		}
	}
}