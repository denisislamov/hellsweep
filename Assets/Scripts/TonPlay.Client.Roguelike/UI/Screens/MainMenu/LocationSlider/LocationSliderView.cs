using System.Collections.Generic;
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
		
		[SerializeField] private List<GameObject> _locationView;
		public List<GameObject> LocationView => _locationView;
		
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

		public void SetLocationView(int index)
		{
			if (index < 0 || index >= _locationView.Count)
			{
				Debug.LogWarning($"Index {index} is out of range");
				return;
			}
			
			for (var i = 0; i < _locationView.Count; i++)
			{
				_locationView[i].SetActive(i == index);
			}
		}
	}
}