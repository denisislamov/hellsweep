using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Extensions;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ProjectileExplodeOnTimerSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<ProjectileComponent>()
						.Inc<ExplodeOnTimerComponent>()
						.Inc<PositionComponent>()
						.Exc<DestroyComponent>()
						.Exc<InactiveComponent>()
						.End();

			var destroyPool = world.GetPool<DestroyComponent>();
			var explodePool = world.GetPool<ExplodeOnTimerComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var explode = ref explodePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				explode.TimeLeft -= Time.deltaTime;
				
				if (explode.TimeLeft < 0)
				{
					var explosionEntity = world.NewEntity();
					explosionEntity.AddPositionComponent(position.Position);
					explosionEntity.AddExplosionComponent(explode.DamageProvider, CollisionAreaFactory.Create(explode.CollisionConfig), explode.CollisionLayerMask);
					explosionEntity.AddStackTryApplyDamageComponent();
					explosionEntity.AddBlockApplyDamageTimerComponent();

					destroyPool.Add(entityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}