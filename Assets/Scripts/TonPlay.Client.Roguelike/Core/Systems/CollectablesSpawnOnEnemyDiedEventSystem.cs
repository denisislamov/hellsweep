using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

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
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var diedEnemyPool = world.GetPool<EnemyDiedEvent>();

			var filter = world.Filter<EnemyDiedEvent>().End();

			foreach (var entityId in filter)
			{
				ref var diedEnemy = ref diedEnemyPool.Get(entityId);

				var collectables = diedEnemy.EnemyConfig.CollectablesIdsOnDeath;

				if (collectables == null || collectables.Count == 0)
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
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}
	}
}