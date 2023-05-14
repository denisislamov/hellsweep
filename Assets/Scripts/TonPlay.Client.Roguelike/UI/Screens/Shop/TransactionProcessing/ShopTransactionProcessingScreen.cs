using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingScreen : Screen<IScreenContext>
	{
		public class Factory : ScreenFactory<IScreenContext, ShopTransactionProcessingScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}