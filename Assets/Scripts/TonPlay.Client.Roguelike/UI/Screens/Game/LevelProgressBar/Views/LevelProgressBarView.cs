using System;
using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Views
{
	public class LevelProgressBarView : View, ILevelProgressBarView
	{
		[SerializeField]
		private TextMeshProUGUI _levelText;

		[SerializeField]
		private GameObject[] _stripes;

		public void SetSize(float size)
		{
			var flooredSize = Mathf.FloorToInt(size*_stripes.Length);

			for (var i = 1; i <= _stripes.Length; i++)
			{
				var stripe = _stripes[i - 1];

				var active = flooredSize >= i;

				if (active != stripe.activeSelf)
				{
					stripe.SetActive(active);
				}
			}
		}

		public void SetLevelText(string text)
		{
			_levelText.SetText(text);
		}
	}
}