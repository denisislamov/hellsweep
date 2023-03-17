using System;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Systems.Enemies;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class InvertRegularEnemyRotationLookAtTargetOnBossAppearingSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<LookAtTargetComponent>()
						.Inc<RegularEnemy>()
						.Inc<RotationComponent>()
						.Inc<PositionComponent>()
						.Inc<TargetComponent>()
						.End();
			var rotationPool = world.GetPool<RotationComponent>();

			var invertRotationOnBossAppearing = IsInversionRequiredOnBossAppearing(world, systems.GetShared<ISharedData>());

			if (!invertRotationOnBossAppearing) return;

			foreach (var entityId in filter)
			{
				ref var rotation = ref rotationPool.Get(entityId);

				rotation.Direction *= -1;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		
		private bool IsInversionRequiredOnBossAppearing(EcsWorld world, ISharedData sharedData)
		{
			var wavesConfigProvider = sharedData.EnemyWavesConfigProvider;
			var enemyConfigProvider = sharedData.EnemyConfigProvider;
			var timePool = world.GetPool<GameTimeComponent>();
			var gameEntityId = world.Filter<GameComponent>().End().GetRawEntities()[0];
			ref var time = ref timePool.Get(gameEntityId);
			var timespan = TimeSpan.FromSeconds(time.Time);
			var nextWave = wavesConfigProvider.Get(timespan.Ticks)?.Next();
			var nextWaveTimeSpan = TimeSpan.FromTicks(nextWave?.StartTimingTicks ?? 0);
			var secondsDifference = (nextWaveTimeSpan - timespan).TotalSeconds;
			var nextWaveHasBoss = false;
			var bossFilter = world.Filter<BossEnemy>().End();
			var bossExists = bossFilter.GetEntitiesCount() > 0;
			for (var i = 0; i < nextWave?.Waves?.Count; i++)
			{
				if (enemyConfigProvider.Get(nextWave.Waves[i].EnemyId).EnemyType == EnemyType.Boss)
				{
					nextWaveHasBoss = true;
					break;
				}
			}

			if (!bossExists &&
				!nextWaveHasBoss ||
				secondsDifference > EnemyWaveSpawnSystem.PREPARE_BEFORE_BOSS_SPAWN_SECONDS)
			{
				return false;
			}

			return true;
		}
	}
}