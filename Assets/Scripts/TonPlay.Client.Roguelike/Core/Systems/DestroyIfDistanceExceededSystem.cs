using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyIfDistanceExceededSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<DestroyIfDistanceExceededComponent>().Inc<PositionComponent>().Exc<InactiveComponent>().End();
			var pool = world.GetPool<DestroyIfDistanceExceededComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (Vector2.Distance(component.StartPosition, position.Position) > component.Distance)
				{
					pool.Del(entityId);
					destroyPool.AddOrGet(entityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}