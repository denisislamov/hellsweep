using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopResourcePopupView : View, IShopResourcePopupView
	{
		[SerializeField]
		private TMP_Text _titleText;
		
		[SerializeField]
		private TMP_Text _amountText;
		
		[SerializeField]
		private TMP_Text _priceText;
		
		[SerializeField]
		private TMP_Text _rarityText;

		[SerializeField]
		private RawImage[] _gradients;

		[SerializeField]
		private ButtonView _buyButtonView;
		
		[SerializeField]
		private ButtonView _closeButtonView;

		[SerializeField]
		private Image _icon;
		
		[SerializeField]
		private Image[] _rarityPanels;

		public IButtonView BuyButtonView => _buyButtonView;
		
		public IButtonView CloseButtonView => _closeButtonView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetAmountText(string text)
		{
			_amountText.SetText(text);
		}
		
		public void SetPriceText(string text)
		{
			_priceText.SetText(text);
		}
		
		public void SetRarityText(string text)
		{
			_rarityText.SetText(text);
		}

		public void SetIcon(Sprite sprite)
		{
			_icon.sprite = sprite;
		}
		
		public void SetPanelsColor(Color color)
		{
			foreach (var image in _rarityPanels)
			{
				image.color = color;
			}
		}

		public void SetBackgroundGradientMaterial(Material material)
		{
			foreach (var image in _gradients)
			{
				image.material = material;
			}
		}
	}
}