using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class UpdateWaveDataOnEnemyDeathSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			
			var filter = world.Filter<EnemyComponent>().Inc<DeadComponent>().End();
			var gameFilter = world.Filter<GameComponent>().Inc<GameTimeComponent>().Inc<WavesDataComponent>().End();
			
			var enemyPool = world.GetPool<EnemyComponent>();
			var deadEnemiesPool = world.GetPool<WavesDataComponent>();

			foreach (var gameEntityId in gameFilter)
			{
				ref var wavesData = ref deadEnemiesPool.Get(gameEntityId);
				
				foreach (var entityId in filter)
				{
					ref var enemy = ref enemyPool.Get(entityId);

					wavesData.WavesEnemiesKilledAmount[enemy.WaveId]++;
					wavesData.WavesEnemiesSpawnedAmount[enemy.WaveId]--;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}
	}
}