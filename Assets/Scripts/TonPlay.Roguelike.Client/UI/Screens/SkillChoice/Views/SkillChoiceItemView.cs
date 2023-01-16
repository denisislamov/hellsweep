using System;
using DG.Tweening;
using TMPro;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views
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
				_activeLevelsImages[i].gameObject.SetActive(level - i >= 0);
			}
		}
		
		public void SetNextLevel(int level)
		{
			var activeImage = _activeLevelsImages[level];
			activeImage.gameObject.SetActive(true);
			activeImage.color = new Color(1, 1, 1, 0);

			_sequence = DOTween.Sequence();
			_sequence.Append(activeImage.DOColor(Color.white, 0.5f));
			_sequence.Append(activeImage.DOColor(new Color(1, 1, 1, 0), 0.5f));
			_sequence.SetLoops(-1);
		}
		
		public void SetMaxLevel(int level)
		{
			for (var i = 0; i < _activeLevelsImages.Length; i++)
			{
				_backgroundLevelsImages[i].gameObject.SetActive(level - i >= 0);
			}
		}

		private void OnDisable()
		{
			_sequence.Kill();
			_sequence = null;
		}
	}
}