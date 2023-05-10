using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackPopupView : IView
	{
		IButtonView BuyButtonView { get; }
		
		IButtonView CloseButtonView { get; }
		
		IShopPackItemCollectionView ItemCollectionView { get; }
		
		void SetTitleText(string text);
		
		void SetDescriptionText(string text);
		
		void SetPriceText(string text);
		
		void SetRarityText(string text);

		void SetPanelsColor(Color color);

		void SetBackgroundGradientMaterial(Material gradient);
	}
}