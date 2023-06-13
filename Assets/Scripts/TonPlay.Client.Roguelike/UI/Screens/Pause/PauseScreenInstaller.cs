using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	[CreateAssetMenu(fileName = nameof(PauseScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(PauseScreenInstaller))]
	public class PauseScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IPauseScreenContext, PauseScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<PauseScreenContext, PauseScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IPauseScreenContext, PauseScreen>>()
						.To<PauseScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<PauseScreenContext, PauseScreen>>()
						.To<PauseScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IPauseScreenView, IPauseScreenContext, PauseScreenPresenter, PauseScreenPresenter.Factory>();
		}
	}
}