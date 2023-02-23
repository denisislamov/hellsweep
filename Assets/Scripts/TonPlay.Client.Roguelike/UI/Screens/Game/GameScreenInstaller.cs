using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	[CreateAssetMenu(fileName = nameof(GameScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(GameScreenInstaller))]
	public class GameScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private Canvas _pooledItemsContainerPrefab;

		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IGameScreenContext, GameScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<GameScreenContext, GameScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IGameScreenContext, GameScreen>>()
						.To<GameScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<GameScreenContext, GameScreen>>()
						.To<GameScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IGameView, IGameScreenContext, GamePresenter, GamePresenter.Factory>().FromNew();
			subContainer.BindFactory<IProgressBarView, IProgressBarContext, ProgressBarPresenter, ProgressBarPresenter.Factory>().FromNew();
			subContainer.BindFactory<ITimerView, ITimerContext, TimerPresenter, TimerPresenter.Factory>().FromNew();
			subContainer.BindFactory<IMatchScoreView, IMatchScoreContext, MatchScorePresenter, MatchScorePresenter.Factory>().FromNew();
			subContainer.BindFactory<IDebugView, IScreenContext, DebugPresenter, DebugPresenter.Factory>().FromNew();
			subContainer.BindFactory<ILevelProgressBarView, ILevelProgressBarContext, LevelProgressBarPresenter, LevelProgressBarPresenter.Factory>().FromNew();
		}
	}
}