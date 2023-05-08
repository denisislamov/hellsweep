using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopResourceView : CollectionItemView, IShopResourceView
	{
		[SerializeField]
		private TMP_Text _titleText;
		
		[SerializeField]
		private TMP_Text _amountText;

		[SerializeField]
		private RawImage[] _gradients;

		[SerializeField]
		private ButtonView _buttonView;

		[SerializeField]
		private Image _icon;

		public IButtonView ButtonView => _buttonView;
		
		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetAmountText(string text)
		{
			_amountText.SetText(text);
		}

		public void SetIcon(Sprite sprite)
		{
			_icon.sprite = sprite;
		}

		public void SetBackgroundGradientMaterial(Material material)
		{
			foreach (var rawImage in _gradients)
			{
				rawImage.material = material;
			}
		}
	}
}