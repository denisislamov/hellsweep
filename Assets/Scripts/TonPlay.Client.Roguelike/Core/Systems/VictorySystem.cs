using System;
using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Victory;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class VictorySystem : IEcsRunSystem
	{
		private readonly IUIService _uiService;
		
		public VictorySystem(IUIService uiService)
		{
			_uiService = uiService;
		}
		
		public void Run(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			var lastWave = sharedData.EnemyWavesConfigProvider.Last;

			if (lastWave == null)
			{
				return;
			}
			
			var gameTime = TimeSpan.FromSeconds(sharedData.GameModel.GameTime.Value);

			if (lastWave.StartTimingTicks > gameTime.Ticks)
			{
				return;
			}

			var world = systems.GetWorld();
			var wavesDataEntityId = world.Filter<WavesDataComponent>().End().GetRawEntities()[0];
			var wavesData = world.GetPool<WavesDataComponent>().Get(wavesDataEntityId);

			if (wavesData.WavesEnemiesKilledAmount == null)
			{
				return;
			}

			var enemiesLeft = false;
			for (int i = 0; i < lastWave.Waves.Count; i++)
			{
				var waveConfig = lastWave.Waves[i];

				if (!wavesData.WavesEnemiesKilledAmount.ContainsKey(waveConfig.Id))
				{
					enemiesLeft = true;
					break;
				}
				
				var leftEnemiesAmount = waveConfig.MaxSpawnedQuantity - wavesData.WavesEnemiesKilledAmount[waveConfig.Id];

				if (leftEnemiesAmount > 0)
				{
					enemiesLeft = true;
					break;
				}
			}

			if (!enemiesLeft)
			{
				_uiService.Open<VictoryScreen, VictoryScreenContext>(new VictoryScreenContext());
			}
		}
	}
}