using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	[CreateAssetMenu(fileName = nameof(ShopResourcesScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopResourcesScreenInstaller))]
	public class ShopResourcesScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopResourcesScreenContext, ShopResourcesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopResourcesScreenContext, ShopResourcesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopResourcesScreenContext, ShopResourcesScreen>>()
						.To<ShopResourcesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopResourcesScreenContext, ShopResourcesScreen>>()
						.To<ShopResourcesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopResourcesView, IShopResourcesScreenContext, ShopResourcesPresenter, ShopResourcesPresenter.Factory>();
		}
	}
}