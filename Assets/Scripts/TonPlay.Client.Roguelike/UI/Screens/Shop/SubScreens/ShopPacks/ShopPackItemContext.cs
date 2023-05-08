using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackItemContext : ScreenContext, IShopPackItemContext
	{
		public Sprite Icon { get; }
		public Material BackgroundGradientMaterial { get; }
		public string AmountText { get; }
		
		public ShopPackItemContext(string amountText, Sprite icon, Material backgroundGradientMaterial)
		{
			Icon = icon;
			BackgroundGradientMaterial = backgroundGradientMaterial;
			AmountText = amountText;
		}
	}
}