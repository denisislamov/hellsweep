using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces
{
	public interface IShopTransactionProcessingScreenContext : IScreenContext
	{
		string TonkeeperUrl { get; }
		System.Action CloseButtonClickCallback { get; }
	}
}