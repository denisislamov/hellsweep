using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment
{
	public class ShopSuccessPaymentPopupView : View, IShopSuccessPaymentView
	{
		[SerializeField]
		private ShopRewardItemCollectionView _itemCollectionView;

		[SerializeField]
		private ButtonView _okButtonView;
		
		[SerializeField]
		private ButtonView _closeButtonView;

		public IButtonView OkButtonView => _okButtonView;
		
		public IButtonView CloseButtonView => _closeButtonView;

		public IShopRewardItemCollectionView ItemCollectionView => _itemCollectionView;
	}
}