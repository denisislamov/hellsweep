using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	public class PlayerHealthBarView : ProgressBarView, IPlayerHealthBarView
	{
		[SerializeField]
		private RectTransform _selfTransform;
		
		public void SetPosition(Vector2 position)
		{
			if (_selfTransform == null) return;
			
			_selfTransform.anchoredPosition = position;
		}
	}
}