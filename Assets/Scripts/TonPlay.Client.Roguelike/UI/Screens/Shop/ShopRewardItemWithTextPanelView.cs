using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopRewardItemWithTextPanelView : ShopRewardItemView, IShopRewardItemWithTextPanelView
	{
		[SerializeField]
		private Image _textPanelImage;
		
		public void SetTextPanelColor(Color color)
		{
			_textPanelImage.color = color;
		}
	}
}