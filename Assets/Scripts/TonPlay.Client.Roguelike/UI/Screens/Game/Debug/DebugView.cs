using TMPro;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug
{
	public class DebugView : View, IDebugView
	{
		[SerializeField]
		private TextMeshProUGUI _framerateText;
		
		[SerializeField]
		private TMP_Text _enemyMovementCollisionCount;

		public void SetFramerateText(string text)
		{
			_framerateText.SetText(text);
		}
		
		public void SetEnemyMovementCollisionCount(int value)
		{
			_enemyMovementCollisionCount.SetText(value);
		}
	}
}