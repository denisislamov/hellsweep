using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views
{
	public class FillImageProgressBarView : View, IProgressBarView
	{
		[SerializeField]
		private Image _bar;

		public void SetSize(float size)
		{
			_bar.fillAmount = size;
		}
	}
}