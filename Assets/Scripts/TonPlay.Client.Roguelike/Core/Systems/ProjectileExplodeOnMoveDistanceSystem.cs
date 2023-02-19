using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ProjectileExplodeOnMoveDistanceSystem : IEcsRunSystem
	{
		private readonly int _layerMask;

		public ProjectileExplodeOnMoveDistanceSystem()
		{
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

			foreach (var entityId in filter)
			{
				ref var explodeOnMove = ref explodePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (Vector2.Distance(position.Position, explodeOnMove.StartPosition) >= explodeOnMove.DistanceToExplode)
				{
					var explosionEntity = world.NewEntity();
					explosionEntity.AddPositionComponent(position.Position);
					explosionEntity.AddExplosionComponent(explodeOnMove.DamageProvider, explodeOnMove.CollisionConfig, _layerMask);
					explosionEntity.AddStackTryApplyDamageComponent();
					explosionEntity.AddBlockApplyDamageTimerComponent();

					destroyPool.Add(entityId);
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}