using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine.Profiling;

namespace TonPlay.Roguelike.Client.Core.Systems
{ 
	public class ProjectileCollisionSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private List<int> _overlappedEntities = new List<int>(32);
		
		public ProjectileCollisionSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();

			var filter = world.Filter<ProjectileComponent>()
							  .Inc<DamageOnCollisionComponent>()
							  .Inc<PositionComponent>()
							  .Inc<CollisionComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			
			var collisionPool = world.GetPool<CollisionComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var damagePool = world.GetPool<DamageOnCollisionComponent>();
			var applyDamagePool = world.GetPool<ApplyDamageComponent>();
			var hasCollidedPool = world.GetPool<HasCollidedComponent>();
			
			foreach (var entityId in filter)
			{
				ref var collision = ref collisionPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var damage = ref damagePool.Get(entityId);
				
				var count = _overlapExecutor.Overlap(
					position.Position, 
					collision.CollisionAreaConfig, 
					ref _overlappedEntities, 
					collision.LayerMask);
				
				for (int i = 0; i < count; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];
					if (applyDamagePool.Has(overlappedEntityId))
					{
						ref var applyDamage = ref applyDamagePool.Get(overlappedEntityId);
						applyDamage.Damage += damage.Damage;
					}
					else
					{
						ref var applyDamage = ref applyDamagePool.Add(overlappedEntityId);
						applyDamage.Damage = damage.Damage;
					}
				}

				if (count > 0)
				{
					hasCollidedPool.Add(entityId);
				}
				
				_overlappedEntities.Clear();
			}
#region Profiling End
			Profiler.EndSample();
#endregion 
		}
	}
}