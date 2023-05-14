using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopRewardItemContext : ScreenContext, IShopRewardItemContext
	{
		public Sprite Icon { get; }
		public Material BackgroundGradientMaterial { get; }
		public string AmountText { get; }
		
		public ShopRewardItemContext(string amountText, Sprite icon, Material backgroundGradientMaterial)
		{
			Icon = icon;
			BackgroundGradientMaterial = backgroundGradientMaterial;
			AmountText = amountText;
		}
	}
}