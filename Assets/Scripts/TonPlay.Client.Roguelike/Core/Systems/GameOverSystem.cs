using Leopotam.EcsLite;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

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
#region Profiling Begin

			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);

#endregion
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var playerFilter = world.Filter<PlayerComponent>()
									.Inc<DeadComponent>()
									.Exc<OpenedUIComponent>()
									.End();

			var openedUiPool = world.GetPool<OpenedUIComponent>();

			foreach (var entityId in playerFilter)
			{
				openedUiPool.Add(entityId);

				_uiService.Open<DefeatGameScreen, IDefeatGameScreenContext>(new DefeatGameScreenContext());
				
				PauseGame(sharedData);
			}

#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
		}
		
		private static void PauseGame(ISharedData sharedData)
		{
			var gameData = sharedData.GameModel.ToData();
			gameData.Paused = true;
			sharedData.GameModel.Update(gameData);
		}
	}
}