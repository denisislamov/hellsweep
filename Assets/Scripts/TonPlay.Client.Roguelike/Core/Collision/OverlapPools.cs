using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class OverlapPools : IOverlapPools
	{
		private static readonly OverlapPools _cached = new OverlapPools();
		
		public EcsPool<InactiveComponent> InactivePool { get; private set; }
		public EcsPool<DeadComponent> DeadPool { get; private set; }
		public EcsPool<UsedComponent> UsedPool { get; private set; }
		public EcsPool<LayerComponent> LayerPool { get; private set; }
		public EcsPool<DestroyComponent> DestroyPool { get; private set; }
		public EcsPool<PositionComponent> PositionPool { get; private set; }
		public EcsPool<CollisionComponent> CollisionPool { get; private set; }

		public IOverlapPools SetPools(
			EcsPool<InactiveComponent> inactivePool,
			EcsPool<DeadComponent> deadPool,
			EcsPool<UsedComponent> usedPool,
			EcsPool<LayerComponent> layerPool,
			EcsPool<DestroyComponent> destroyPool,
			EcsPool<PositionComponent> positionPool,
			EcsPool<CollisionComponent> collisionPool)
		{
			InactivePool = inactivePool;
			DeadPool = deadPool;
			UsedPool = usedPool;
			LayerPool = layerPool;
			DestroyPool = destroyPool;
			PositionPool = positionPool;
			CollisionPool = collisionPool;
			return this;
		}

		public static IOverlapPools Create(EcsWorld world)
		{
			return _cached.SetPools(
				world.GetPool<InactiveComponent>(),
				world.GetPool<DeadComponent>(),
				world.GetPool<UsedComponent>(),
				world.GetPool<LayerComponent>(),
				world.GetPool<DestroyComponent>(),
				world.GetPool<PositionComponent>(),
				world.GetPool<CollisionComponent>());
		}
	}
}