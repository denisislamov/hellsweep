using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class GameOverSystem : IEcsRunSystem
	{
		private readonly IUIService _uiService;

		public GameOverSystem(IUIService uiService)
		{
			_uiService = uiService;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			if (sharedData.GameModel.DebugForcedLose)
			{
				FinishGame(sharedData);
			}
			
			var playerFilter = world.Filter<PlayerComponent>()
									.Inc<DeadComponent>()
									.Exc<OpenedUIComponent>()
									.End();

			var openedUiPool = world.GetPool<OpenedUIComponent>();

			foreach (var entityId in playerFilter)
			{
				openedUiPool.Add(entityId);

				FinishGame(sharedData);
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private void FinishGame(ISharedData sharedData)
		{
			_uiService.Open<DefeatGameScreen, IDefeatGameScreenContext>(new DefeatGameScreenContext());

			PauseGame(sharedData);
		}

		private static void PauseGame(ISharedData sharedData)
		{
			var gameData = sharedData.GameModel.ToData();
			gameData.Paused = true;
			sharedData.GameModel.Update(gameData);
		}
	}
}