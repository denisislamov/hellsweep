using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider
{
	public class LocationSliderView : View, ILocationSliderView
	{
		[SerializeField]
		private ButtonView _leftButton;

		[SerializeField]
		private ButtonView _rightButton;

		[SerializeField]
		private TextMeshProUGUI _titleText;

		[SerializeField]
		private TextMeshProUGUI _subtitleText;

		[SerializeField]
		private GameObject[] _activatableObjects;

		[SerializeField]
		private Image _icon;

		public IButtonView LeftButton => _leftButton;
		public IButtonView RightButton => _rightButton;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}

		public void SetSubtitleText(string text)
		{
			_subtitleText.SetText(text);
		}

		public void SetIcon(Sprite sprite)
		{
			_icon.sprite = sprite;
		}
	}
}