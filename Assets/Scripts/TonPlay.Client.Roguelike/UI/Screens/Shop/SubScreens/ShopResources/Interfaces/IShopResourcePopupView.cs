using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces
{
	public interface IShopResourcePopupView : IView
	{
		IButtonView BuyButtonView { get; }
		
		IButtonView CloseButtonView { get; }
		
		void SetTitleText(string text);
		
		void SetAmountText(string text);
		
		void SetPriceText(string text);
		
		void SetRarityText(string text);

		void SetIcon(Sprite sprite);

		void SetPanelsColor(Color color);
		
		void SetBackgroundGradientMaterial(Material material);
	}
}