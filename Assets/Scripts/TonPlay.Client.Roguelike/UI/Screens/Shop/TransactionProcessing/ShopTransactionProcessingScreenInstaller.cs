using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	[CreateAssetMenu(fileName = nameof(ShopTransactionProcessingScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopTransactionProcessingScreenInstaller))]
	public class ShopTransactionProcessingScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopTransactionProcessingScreenContext, ShopTransactionProcessingScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopTransactionProcessingScreenContext, ShopTransactionProcessingScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopTransactionProcessingScreenContext, ShopTransactionProcessingScreen>>()
						.To<ShopTransactionProcessingScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopTransactionProcessingScreenContext, ShopTransactionProcessingScreen>>()
						.To<ShopTransactionProcessingScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
			
			subContainer.BindFactory<IShopTransactionProcessingView, IShopTransactionProcessingScreenContext, ShopTransactionProcessingPresenter, ShopTransactionProcessingPresenter.Factory>();
		}
	}
}