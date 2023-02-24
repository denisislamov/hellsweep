using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class CollectablesSpawnOnEnemyDiedEventSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly ICollectableEntityFactory _collectablesEntityFactory;

		private ISharedData _sharedData;

		public CollectablesSpawnOnEnemyDiedEventSystem(ICollectableEntityFactory collectablesEntityFactory)
		{
			_collectablesEntityFactory = collectablesEntityFactory;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var diedEnemyPool = world.GetPool<EnemyDiedEvent>();

			var filter = world.Filter<EnemyDiedEvent>().End();

			foreach (var entityId in filter)
			{
				ref var diedEnemy = ref diedEnemyPool.Get(entityId);

				var collectables = diedEnemy.EnemyConfig
											.RandomCollectableDrops
											.Select(_ => _.Drop())
											.Where(_ => !string.IsNullOrEmpty(_))
											.ToArray();

				if (collectables.Length == 0)
				{
					world.DelEntity(entityId);
					continue;
				}

				foreach (var collectableId in collectables)
				{
					var collectableConfig = _sharedData.CollectablesConfigProvider.Get(collectableId);

					_collectablesEntityFactory.Create(world, collectableConfig, diedEnemy.Position);
				}

				diedEnemyPool.Del(entityId);

				world.DelEntity(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}