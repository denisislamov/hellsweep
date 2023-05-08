using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces
{
	public interface IShopResourceView : ICollectionItemView
	{
		IButtonView ButtonView { get; }
		
		void SetTitleText(string text);

		void SetIcon(Sprite sprite);
		
		void SetBackgroundGradientMaterial(Material material);
	}
}