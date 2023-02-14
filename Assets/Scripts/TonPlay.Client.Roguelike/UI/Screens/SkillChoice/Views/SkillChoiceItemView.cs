using System;
using DG.Tweening;
using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views
{
	public class SkillChoiceItemView : CollectionItemView, ISkillChoiceItemView
	{
		[SerializeField]
		private TextMeshProUGUI _title;
		
		[SerializeField]
		private TextMeshProUGUI _description;
		
		[SerializeField]
		private Image _icon;

		[SerializeField]
		private Image[] _activeLevelsImages;
		
		[SerializeField]
		private Image[] _backgroundLevelsImages;

		[SerializeField]
		private Image[] _colorBackgroundImages;

		[SerializeField]
		private Button _button;

		private Sequence _sequence;

		public IObservable<Unit> OnButtonClick => _button.OnClickAsObservable();

		public void SetTitleText(string text)
		{
			_title.SetText(text);
		}
		
		public void SetDescriptionText(string text)
		{
			_description.SetText(text);
		}
		
		public void SetIcon(Sprite icon)
		{
			_icon.sprite = icon;
		}
		
		public void SetCurrentLevel(int level)
		{
			for (var i = 0; i < _activeLevelsImages.Length; i++)
			{
				_activeLevelsImages[i].gameObject.SetActive(level - i - 1 >= 0);
				_activeLevelsImages[i].color = Color.white;
			}
		}
		
		public void SetNextLevel(int level)
		{
			var activeImage = _activeLevelsImages[level - 1];
			activeImage.gameObject.SetActive(true);
			activeImage.color = new Color(1, 1, 1, 0);

			_sequence = DOTween.Sequence();
			_sequence.Append(activeImage.DOColor(Color.white, 0.5f));
			_sequence.Append(activeImage.DOColor(new Color(1, 1, 1, 0), 0.5f));
			_sequence.SetLoops(-1);
		}
		
		public void SetMaxLevel(int level)
		{
			for (var i = 0; i < _backgroundLevelsImages.Length; i++)
			{
				_backgroundLevelsImages[i].gameObject.SetActive(level - i - 1>= 0);
			}
		}
		
		public void SetBackgroundColor(Color color)
		{
			for (var i = 0; i < _colorBackgroundImages.Length; i++)
			{
				_colorBackgroundImages[i].color = color;
			}
		}
		
		public void SetTitleTextColor(Color color)
		{
			_title.color = color;
		}
		
		public void SetLevelIcon(Sprite icon)
		{
			for (var i = 0; i < _activeLevelsImages.Length; i++)
			{
				_activeLevelsImages[i].sprite = icon;
			}
		}

		private void OnDisable()
		{
			_sequence.Kill();
			_sequence = null;
		}
	}
}