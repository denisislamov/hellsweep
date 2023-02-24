using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DestroyOnCollisionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>()
							  .Inc<DestroyOnCollisionComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			var hasCollidedPool = world.GetPool<HasCollidedComponent>();
			var pool = world.GetPool<DestroyComponent>();
			var layerPool = world.GetPool<LayerComponent>();
			var destroyOnCollisionPool = world.GetPool<DestroyOnCollisionComponent>();

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref hasCollidedPool.Get(entityId);
				ref var destroyOnCollision = ref destroyOnCollisionPool.Get(entityId);

				if (hasCollided.CollidedEntityIds.Count > 0)
				{
					for (int i = 0; i < hasCollided.CollidedEntityIds.Count; i++)
					{
						var collidedEntityId = hasCollided.CollidedEntityIds[i];
						ref var layer = ref layerPool.Get(collidedEntityId);

						if (DoesLayerMaskContainsLayer(destroyOnCollision.LayerMask, layer.Layer))
						{
							pool.AddOrGet(entityId);
							break;
						}
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private static bool DoesLayerMaskContainsLayer(int layerMask, int layer)
		{
			return (layerMask & (1 << layer)) != 0;
		}
	}
}