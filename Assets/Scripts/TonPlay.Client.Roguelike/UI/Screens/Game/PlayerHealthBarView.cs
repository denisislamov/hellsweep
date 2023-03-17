using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	public class PlayerHealthBarView : ProgressBarView, IPlayerHealthBarView
	{
		[SerializeField]
		private RectTransform _selfTransform;
		
		[SerializeField]
		private RectTransform _rootCanvasTransform;
		
		public void SetPosition(Vector2 position)
		{
			if (_selfTransform == null || _rootCanvasTransform == null) return;

			var canvasRect = _rootCanvasTransform.rect;
			var selfTransformRect = _selfTransform.rect;
			
			_selfTransform.anchoredPosition = new Vector2(
				position.x - Screen.width * 0.5f  ,
				position.y - Screen.height * 0.5f );
		}
	}
}