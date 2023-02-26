using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class BossShooterRicochetProjectileOffTheArenaSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			
			var world = systems.GetWorld();
			var arenaFilter = world
							 .Filter<Arena>()
							 .Inc<PositionComponent>()
							 .Exc<DestroyComponent>()
							 .End();
			
			if (arenaFilter.GetEntitiesCount() <= 0)
			{
				return;
			}

			var filter = world
						.Filter<HasCollidedComponent>()
						.Inc<ProjectileComponent>()
						.Inc<RicochetOffTheArenaComponent>()
						.Inc<MovementComponent>()
						.Inc<PositionComponent>()
						.Inc<RotationComponent>()
						.Exc<DestroyComponent>()
						.End();

			var hasCollidedPool = world.GetPool<HasCollidedComponent>();
			var layerPool = world.GetPool<LayerComponent>();
			var ricochetPool = world.GetPool<RicochetOffTheArenaComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var rotationPool = world.GetPool<RotationComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			
			var arenaEntityId = arenaFilter.GetRawEntities()[0];
			var arenaPosition = positionPool.Get(arenaEntityId);

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref hasCollidedPool.Get(entityId);
				
				if (hasCollided.CollidedEntityIds.Count <= 0) continue;
				
				ref var ricochet = ref ricochetPool.Get(entityId);
			
				for (var i = 0; i < hasCollided.CollidedEntityIds.Count; i++)
				{
					var collidedEntityId = hasCollided.CollidedEntityIds[i];
			
					ref var collidedEntityLayer = ref layerPool.Get(collidedEntityId);
			
					if (LayerMaskExt.ContainsLayer(ricochet.LayerMask, collidedEntityLayer.Layer))
					{
						ref var projectileMovement = ref movementPool.Get(entityId);
						ref var projectileRotation = ref rotationPool.Get(entityId);
						ref var projectilePosition = ref positionPool.Get(entityId);

						var dirToArena = projectilePosition.Position - arenaPosition.Position;

						var newDirection = new Vector2(
							Mathf.Abs(dirToArena.x) < Mathf.Abs(dirToArena.y) ? projectileMovement.Direction.x : -projectileMovement.Direction.x,
							Mathf.Abs(dirToArena.x) >= Mathf.Abs(dirToArena.y) ? projectileMovement.Direction.y : -projectileMovement.Direction.y);

						projectileMovement.Direction = newDirection;
						projectileRotation.Direction = projectileMovement.Direction;
						
						break;
					}
				}
			}
			
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}