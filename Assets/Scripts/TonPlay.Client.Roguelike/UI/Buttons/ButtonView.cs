using System;
using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ButtonView : View, IButtonView
	{
		[SerializeField]
		private Button _button;

		[SerializeField]
		private TextMeshProUGUI _text;

		[SerializeField]
		private GameObject[] _activatableObjects;

		public override void Show()
		{
			if (_activatableObjects.Length == 0)
			{
				base.Show();
				return;
			}

			for (var i = 0; i < _activatableObjects.Length; i++)
			{
				_activatableObjects[i].gameObject.SetActive(true);
			}
		}

		public override void Hide()
		{
			if (_activatableObjects.Length == 0)
			{
				base.Hide();
				return;
			}

			for (var i = 0; i < _activatableObjects.Length; i++)
			{
				_activatableObjects[i].gameObject.SetActive(false);
			}
		}

		public IObservable<Unit> OnClick => _button.OnClickAsObservable();

		public void SetText(string text)
		{
			_text.SetText(text);
		}
	}
}