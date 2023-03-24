using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Loading
{
	[CreateAssetMenu(fileName = nameof(LoadingScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(LoadingScreenInstaller))]
	public class LoadingScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IScreenContext, LoadingScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ScreenContext, LoadingScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IScreenContext, LoadingScreen>>()
						.To<LoadingScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ScreenContext, LoadingScreen>>()
						.To<LoadingScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
		}
	}
}