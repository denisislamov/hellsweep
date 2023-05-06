using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	[CreateAssetMenu(fileName = nameof(ShopLootboxesScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopLootboxesScreenInstaller))]
	public class ShopLootboxesScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopLootboxesScreenContext, ShopLootboxesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopLootboxesScreenContext, ShopLootboxesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopLootboxesScreenContext, ShopLootboxesScreen>>()
						.To<ShopLootboxesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopLootboxesScreenContext, ShopLootboxesScreen>>()
						.To<ShopLootboxesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopLootboxesView, IShopLootboxesScreenContext, ShopLootboxesPresenter, ShopLootboxesPresenter.Factory>();
		}
	}
}