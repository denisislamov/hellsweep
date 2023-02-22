using System;
using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class RegularEnemyMoveOutOnBossSpawnSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample("TonPlay.Client.Roguelike.Core.Systems.Enemies.RegularEnemyMoveOutOnBossSpawnSystem");
#endregion
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var wavesConfigProvider = sharedData.EnemyWavesConfigProvider;
			var enemyConfigProvider = sharedData.EnemyConfigProvider;

			var bossFilter = world.Filter<BossEnemy>().End();
			var bossExists = bossFilter.GetEntitiesCount() > 0;

			var gameEntityId = world.Filter<GameComponent>().End().GetRawEntities()[0];

			var timePool = world.GetPool<GameTimeComponent>();
			ref var time = ref timePool.Get(gameEntityId);

			var timespan = TimeSpan.FromSeconds(time.Time);

			var nextWave = wavesConfigProvider
						  .Get(timespan.Ticks)
						  .Next();

			var secondsDifference = TimeSpan.FromTicks(nextWave?.StartTimingTicks ?? 0 - timespan.Ticks).TotalSeconds;

			var nextWaveHasBoss = nextWave?.Waves.Any(_ => enemyConfigProvider.Get(_.EnemyId).EnemyType == EnemyType.Boss);

			if (!bossExists &&
				(!nextWaveHasBoss.HasValue ||
				 !nextWaveHasBoss.Value ||
				 secondsDifference > EnemyWaveSpawnSystem.PREPARE_BEFORE_BOSS_SPAWN_SECONDS))
			{
#region Profiling End
				UnityEngine.Profiling.Profiler.EndSample();
#endregion
				return;
			}

			var filter = world.Filter<RegularEnemy>()
							  .Inc<PositionComponent>()
							  .Inc<MovementComponent>()
							  .Exc<DeadComponent>()
							  .End();

			var playerPosition = systems.GetShared<ISharedData>().PlayerPositionProvider;

			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var movement = ref movementPool.Get(entityId);

				movement.Direction = (position.Position - playerPosition.Position);
				movement.Direction.Normalize();
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}
	}
}