using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views
{
	public class HealthBarView : View, IHealthBarView
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