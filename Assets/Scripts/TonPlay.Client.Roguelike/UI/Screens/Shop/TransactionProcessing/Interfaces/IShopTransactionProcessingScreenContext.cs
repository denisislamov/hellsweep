using TonPlay.Client.Common.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces
{
	public interface IShopTransactionProcessingScreenContext : IScreenContext
	{
		IReadOnlyReactiveProperty<bool> ResponseReceived { get; }
		IReadOnlyReactiveProperty<string> TonkeeperUrl { get; }
		System.Action CloseButtonClickCallback { get; }
	}
}