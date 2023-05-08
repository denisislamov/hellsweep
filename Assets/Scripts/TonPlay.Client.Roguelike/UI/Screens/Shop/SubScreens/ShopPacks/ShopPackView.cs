using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackView : CollectionItemView, IShopPackView
	{
		[SerializeField]
		private TMP_Text _titleText;

		[SerializeField]
		private Image[] _colorPanels;
		
		[SerializeField]
		private RawImage[] _gradients;

		[SerializeField]
		private ButtonView _buttonView;

		[SerializeField]
		private ShopPackItemCollectionView _itemCollectionView;

		public IButtonView ButtonView => _buttonView;
		
		public IShopPackItemCollectionView ItemCollectionView => _itemCollectionView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetBackgroundGradientMaterial(Material material)
		{
			foreach (var rawImage in _gradients)
			{
				rawImage.material = material;
			}
		}
		
		public void SetPanelsColor(Color color)
		{
			foreach (var colorPanel in _colorPanels)
			{
				colorPanel.color = color;
			}
		}
	}
}