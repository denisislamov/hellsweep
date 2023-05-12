using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	[CreateAssetMenu(fileName = nameof(ShopLootboxOpeningScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopLootboxOpeningScreenInstaller))]
	public class ShopLootboxOpeningScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
						.To<ShopLootboxOpeningScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
						.To<ShopLootboxOpeningScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopLootboxOpeningView, IShopLootboxOpeningScreenContext, ShopLootboxOpeningPresenter, ShopLootboxOpeningPresenter.Factory>();
		}
	}
}