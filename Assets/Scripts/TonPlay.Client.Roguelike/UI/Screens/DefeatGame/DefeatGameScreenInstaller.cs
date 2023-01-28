using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.DefeatGame
{
	[CreateAssetMenu(fileName = nameof(DefeatGameScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(DefeatGameScreenInstaller))]
	public class DefeatGameScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();
			
			Bind(subContainer);
			
			Container.Bind<IScreenFactory<IDefeatGameScreenContext, DefeatGameScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
			
			Container.Bind<IScreenFactory<DefeatGameScreenContext, DefeatGameScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}
		
		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<DefeatGameScreenContext, DefeatGameScreen>>()
						.To<DefeatGameScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
			
			subContainer.Bind<IScreenFactory<IDefeatGameScreenContext, DefeatGameScreen>>()
						.To<DefeatGameScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IDefeatGameView, IDefeatGameScreenContext, DefeatGamePresenter, DefeatGamePresenter.Factory>();
			subContainer.BindFactory<ITimerView, ITimerContext, TimerPresenter, TimerPresenter.Factory>();
		}
	}
}