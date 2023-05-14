using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces
{
	public interface IShopSuccessPaymentView : IView
	{
		IButtonView OkButtonView { get; }
		
		IButtonView CloseButtonView { get; }

		IShopRewardItemCollectionView ItemCollectionView { get; }
	}
}