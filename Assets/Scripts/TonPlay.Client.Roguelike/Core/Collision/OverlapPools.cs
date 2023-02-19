using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class OverlapPools : IOverlapPools
	{
		public EcsPool<InactiveComponent> InactivePool { get; }
		public EcsPool<DeadComponent> DeadPool { get; }
		public EcsPool<UsedComponent> UsedPool { get; }
		public EcsPool<LayerComponent> LayerPool { get; }
		public EcsPool<DestroyComponent> DestroyPool { get; }

		public OverlapPools(
			EcsPool<InactiveComponent> inactivePool,
			EcsPool<DeadComponent> deadPool,
			EcsPool<UsedComponent> usedPool,
			EcsPool<LayerComponent> layerPool, 
			EcsPool<DestroyComponent> destroyPool)
		{
			InactivePool = inactivePool;
			DeadPool = deadPool;
			UsedPool = usedPool;
			LayerPool = layerPool;
			DestroyPool = destroyPool;
		}

		public static IOverlapPools Create(EcsWorld world)
		{
			return new OverlapPools(
				world.GetPool<InactiveComponent>(),
				world.GetPool<DeadComponent>(),
				world.GetPool<UsedComponent>(),
				world.GetPool<LayerComponent>(),
				world.GetPool<DestroyComponent>());
		}
	}
}