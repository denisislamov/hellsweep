using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingScreenContext : ScreenContext, IShopTransactionProcessingScreenContext
	{
		public IReadOnlyReactiveProperty<bool> ResponseReceived { get; }
		public IReadOnlyReactiveProperty<string> TonkeeperUrl { get; }
		public Action CloseButtonClickCallback { get; }

		public ShopTransactionProcessingScreenContext(IReadOnlyReactiveProperty<string> tonkeeperUrl, 
													  IReadOnlyReactiveProperty<bool> responseReceived,
													  Action closeButtonClickCallback)
		{
			TonkeeperUrl = tonkeeperUrl;
			CloseButtonClickCallback = closeButtonClickCallback;
			ResponseReceived = responseReceived;
		}
	}
}