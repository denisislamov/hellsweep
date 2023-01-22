using DG.Tweening;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EaseMovementSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world
						.Filter<EaseMovementComponent>()
						.Inc<PositionComponent>()
						.Exc<DestroyComponent>()
						.End();

			var positionPool = world.GetPool<PositionComponent>();
			var easeMovementPool = world.GetPool<EaseMovementComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var easeMovement = ref easeMovementPool.Get(entityId);

				position.Position = DOVirtual.EasedValue(
					easeMovement.FromPosition,
					easeMovement.ToPosition,
					Mathf.Clamp(easeMovement.ActiveTime/easeMovement.Duration, 0f, 1f),
					easeMovement.Ease);

				easeMovement.ActiveTime += Time.deltaTime;
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}