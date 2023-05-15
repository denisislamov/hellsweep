using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingScreenContext : ScreenContext, IShopTransactionProcessingScreenContext
	{
		public string TonkeeperUrl { get; }
		public Action CloseButtonClickCallback { get; }

		public ShopTransactionProcessingScreenContext(string tonkeeperUrl, Action closeButtonClickCallback)
		{
			TonkeeperUrl = tonkeeperUrl;
			CloseButtonClickCallback = closeButtonClickCallback;
		}
	}
}