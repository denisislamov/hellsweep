using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingView : View, IShopTransactionProcessingView
	{
		[SerializeField] 
		private ButtonView _payButtonView;
		
		[SerializeField] 
		private ButtonView _cancelButtonView;

		public IButtonView PayButtonView => _payButtonView;
		public IButtonView CancelButtonView => _cancelButtonView;
	}
}