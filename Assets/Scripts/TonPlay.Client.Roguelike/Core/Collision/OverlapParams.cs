using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class OverlapParams : IOverlapParams
	{
		private static readonly OverlapParams s_Cached = new OverlapParams();

		private readonly SimpleIntHashSet _filteredEntities = new SimpleIntHashSet();

		private EcsWorld _world;
		private EcsFilter _filter;

		public EcsPool<LayerComponent> LayerPool { get; private set; }
		public EcsPool<CollisionComponent> CollisionPool { get; private set; }
		public SimpleIntHashSet FilteredEntities => _filteredEntities;

		private OverlapParams SetPools(
			EcsPool<LayerComponent> layerPool,
			EcsPool<CollisionComponent> collisionPool)
		{
			LayerPool = layerPool;
			CollisionPool = collisionPool;
			return this;
		}

		public EcsWorld.Mask CreateDefaultFilterMask() =>
			_world.Filter<CollisionComponent>()
				  .Inc<LayerComponent>()
				  .Exc<DeadComponent>()
				  .Exc<DestroyComponent>();

		private IOverlapParams SetFilteredEntities(int[] filteredEntities, int entitiesCount)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			_filteredEntities.Clear();

			for (var index = 0; index < entitiesCount; index++)
			{
				var entityId = filteredEntities[index];

				_filteredEntities.Add(entityId);
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
			return this;
		}

		public IOverlapParams SetFilter(EcsFilter filter)
		{
			_filter = filter;
			return this;
		}

		public IOverlapParams Build()
		{
			SetFilteredEntities(_filter.GetRawEntities(), _filter.GetEntitiesCount());
			return this;
		}

		private OverlapParams SetWorld(EcsWorld world)
		{
			_world = world;
			return this;
		}

		public static IOverlapParams Create(EcsWorld world)
		{
			return s_Cached
				  .SetWorld(world)
				  .SetPools(world.GetPool<LayerComponent>(),
					   world.GetPool<CollisionComponent>());
		}
	}
}