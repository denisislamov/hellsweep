using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug
{
	public class DebugView : View, IDebugView
	{
		[SerializeField]
		private TextMeshProUGUI _framerateText;

		public void SetFramerateText(string text)
		{
			_framerateText.SetText(text);
		}
	}
}