using TonPlay.Roguelike.Client.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.MainMenu
{
	[CreateAssetMenu(fileName = nameof(MainMenuScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(MainMenuScreenInstaller))]
	public class MainMenuScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();
			
			Bind(subContainer);
			
			Container.Bind<IScreenFactory<IMainMenuScreenContext, MainMenuScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
			
			Container.Bind<IScreenFactory<MainMenuScreenContext, MainMenuScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}
		
		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IMainMenuScreenContext, MainMenuScreen>>()
						.To<MainMenuScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
			
			subContainer.Bind<IScreenFactory<MainMenuScreenContext, MainMenuScreen>>()
						.To<MainMenuScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IMainMenuView, IMainMenuScreenContext, MainMenuPresenter, MainMenuPresenter.Factory>();
		}
	}
}