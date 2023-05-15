using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces
{
	public interface IShopTransactionProcessingView : IView
	{
		IButtonView PayButtonView { get; }
		
		IButtonView CancelButtonView { get; }
	}
}