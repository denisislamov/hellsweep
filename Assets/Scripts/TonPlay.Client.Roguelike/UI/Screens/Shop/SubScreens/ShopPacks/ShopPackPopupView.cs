using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackPopupView : View, IShopPackPopupView
	{
		[SerializeField]
		private TMP_Text _titleText;
		
		[SerializeField]
		private TMP_Text _descriptionText;
		
		[SerializeField]
		private TMP_Text _priceText;
		
		[SerializeField]
		private TMP_Text _rarityText;

		[SerializeField]
		private ShopPackItemCollectionView _itemCollectionView;

		[SerializeField]
		private ButtonView _buyButtonView;
		
		[SerializeField]
		private ButtonView _closeButtonView;

		[SerializeField]
		private Image[] _rarityPanels;
		
		[SerializeField]
		private RawImage[] _backgroundGradients;

		public IButtonView BuyButtonView => _buyButtonView;
		
		public IButtonView CloseButtonView => _closeButtonView;

		public IShopPackItemCollectionView ItemCollectionView => _itemCollectionView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetDescriptionText(string text)
		{
			_descriptionText.SetText(text);
		}
		
		public void SetPriceText(string text)
		{
			_priceText.SetText(text);
		}
		
		public void SetRarityText(string text)
		{
			_rarityText.SetText(text);
		}
		
		public void SetPanelsColor(Color color)
		{
			foreach (var image in _rarityPanels)
			{
				image.color = color;
			}
		}
		
		public void SetBackgroundGradientMaterial(Material gradient)
		{
			foreach (var image in _backgroundGradients)
			{
				image.material = gradient;
			}
		}
	}
}