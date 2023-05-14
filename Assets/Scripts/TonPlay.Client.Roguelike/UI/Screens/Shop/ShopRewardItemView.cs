using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopRewardItemView : CollectionItemView, IShopRewardItemView
	{
		[SerializeField]
		private Image _iconImage;
		
		[SerializeField]
		private RawImage _backgroundImage;

		[SerializeField]
		private TMP_Text _amountText;
		
		public void SetIcon(Sprite sprite)
		{
			_iconImage.sprite = sprite;
		}
		
		public void SetAmountText(string text)
		{
			_amountText.SetText(text);
		}
		
		public void SetBackgroundGradient(Material material)
		{
			_backgroundImage.material = material;
		}
	}
}