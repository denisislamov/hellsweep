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

		public IObservable<Unit> OnClick => _button.OnClickAsObservable();
		
		public void SetText(string text)
		{
			_text.SetText(text);
		}
	}
}