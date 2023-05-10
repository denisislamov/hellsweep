using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	[CreateAssetMenu(fileName = nameof(ShopResourcePopupScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopResourcePopupScreenInstaller))]
	public class ShopResourcePopupScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopResourcePopupScreenContext, ShopResourcePopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopResourcePopupScreenContext, ShopResourcePopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopResourcePopupScreenContext, ShopResourcePopupScreen>>()
						.To<ShopResourcePopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopResourcePopupScreenContext, ShopResourcePopupScreen>>()
						.To<ShopResourcePopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopResourcePopupView, IShopResourcePopupScreenContext, ShopResourcePopupPresenter, ShopResourcePopupPresenter.Factory>();
		}
	}
}