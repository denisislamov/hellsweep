using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore
{
	public class MatchScoreView : View, IMatchScoreView
	{
		[SerializeField]
		private TextMeshProUGUI _goldText;
		
		[SerializeField]
		private TextMeshProUGUI _deadEnemiesText;
		
		public void SetGoldText(string text)
		{
			_goldText.SetText(text);
		}
		
		public void SetDeadEnemiesText(string text)
		{
			_deadEnemiesText.SetText(text);
		}
	}
}