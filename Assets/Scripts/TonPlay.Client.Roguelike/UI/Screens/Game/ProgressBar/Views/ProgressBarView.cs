using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views
{
	public class ProgressBarView : View, IProgressBarView
	{
		[SerializeField]
		private RectTransform _bar;

		public void SetSize(float size)
		{
			_bar.anchorMax = new Vector2(size, _bar.anchorMax.y);
			_bar.sizeDelta = Vector2.one;
		}
	}
}