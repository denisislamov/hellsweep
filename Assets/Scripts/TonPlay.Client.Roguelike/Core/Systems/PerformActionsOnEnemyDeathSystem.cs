using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PerformActionsOnEnemyDeathSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var configProvider = sharedData.EnemyConfigProvider;
			var filter = world.Filter<EnemyComponent>().Inc<DeadComponent>().End();
			var enemyPool = world.GetPool<EnemyComponent>();

			foreach (var entityId in filter)
			{
				ref var enemy = ref enemyPool.Get(entityId);

				var config = configProvider.Get(enemy.ConfigId);

				if (config.HasProperty<IPerformActionsOnDeadEnemyPropertyConfig>())
				{
					var property = config.GetProperty<IPerformActionsOnDeadEnemyPropertyConfig>();
					for (var i = 0; i < property.Actions.Length; i++)
					{
						var action = property.Actions[i];
						action.Execute(entityId, sharedData);
					}
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}
	}
}