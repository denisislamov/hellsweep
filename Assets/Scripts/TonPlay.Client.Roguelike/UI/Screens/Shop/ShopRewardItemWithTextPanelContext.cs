using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopRewardItemWithTextPanelContext : ShopRewardItemContext, IShopRewardItemWithTextPanelContext
	{
		public Color TextPanelColor { get; }
		
		public ShopRewardItemWithTextPanelContext(string amountText, Sprite icon, Material backgroundGradientMaterial, Color textPanelColor) 
			: base(amountText, icon, backgroundGradientMaterial)
		{
			TextPanelColor = textPanelColor;
		}
	}
}