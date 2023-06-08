using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Extensions;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ProjectileExplodeOnCollisionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>()
							  .Inc<ExplodeOnCollisionComponent>()
							  .Inc<PositionComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			var hasCollidedPool = world.GetPool<HasCollidedComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var explodePool = world.GetPool<ExplodeOnCollisionComponent>();

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref hasCollidedPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var explode = ref explodePool.Get(entityId);

				if (hasCollided.CollidedEntityIds.Count > 0)
				{
					var explosionEntity = world.NewEntity();
					explosionEntity.AddPositionComponent(position.Position);
					explosionEntity.AddExplosionComponent(explode.DamageProvider, CollisionAreaFactory.Create(explode.CollisionConfig), explode.CollisionLayerMask);
					explosionEntity.AddStackTryApplyDamageComponent();
					explosionEntity.AddBlockApplyDamageTimerComponent();

					destroyPool.AddOrGet(entityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}