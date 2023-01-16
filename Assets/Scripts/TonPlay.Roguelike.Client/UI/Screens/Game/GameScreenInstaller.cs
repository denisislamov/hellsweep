using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.ProgressBar;
using TonPlay.Roguelike.Client.UI.Screens.Game.ProgressBar.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	[CreateAssetMenu(fileName = nameof(GameScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(GameScreenInstaller))]
	public class GameScreenInstaller : ScreenInstaller
	{
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

			subContainer.BindFactory<IGameView, IGameScreenContext, GamePresenter, GamePresenter.Factory>();
			
			subContainer.BindFactory<IProgressBarView, IProgressBarContext, ProgressBarPresenter, ProgressBarPresenter.Factory>();
			
			subContainer.BindFactory<ITimerView, ITimerContext, TimerPresenter, TimerPresenter.Factory>();
		}
	}
}