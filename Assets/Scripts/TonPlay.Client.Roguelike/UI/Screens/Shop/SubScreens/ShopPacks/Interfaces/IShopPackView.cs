using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackView : ICollectionItemView
	{
		IButtonView ButtonView { get; }
		
		IShopPackItemCollectionView ItemCollectionView { get; }

		void SetTitleText(string text);
		
		void SetBackgroundGradientMaterial(Material material);

		void SetPanelsColor(Color color);
	}
}