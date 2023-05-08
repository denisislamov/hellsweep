using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackItemView : ICollectionItemView
	{
		void SetIcon(Sprite sprite);

		void SetAmountText(string text);

		void SetBackgroundGradient(Material material);
	}
}