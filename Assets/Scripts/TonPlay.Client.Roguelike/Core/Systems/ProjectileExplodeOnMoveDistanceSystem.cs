using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Extensions;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ProjectileExplodeOnMoveDistanceSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
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

			foreach (var entityId in filter)
			{
				ref var explodeOnMove = ref explodePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (Vector2.Distance(position.Position, explodeOnMove.StartPosition) >= explodeOnMove.DistanceToExplode)
				{
					var explosionEntity = world.NewEntity();
					explosionEntity.AddPositionComponent(position.Position);
					explosionEntity.AddExplosionComponent(explodeOnMove.DamageProvider, CollisionAreaFactory.Create(explodeOnMove.CollisionConfig), explodeOnMove.CollisionLayerMask);
					explosionEntity.AddStackTryApplyDamageComponent();
					explosionEntity.AddBlockApplyDamageTimerComponent();

					destroyPool.Add(entityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}