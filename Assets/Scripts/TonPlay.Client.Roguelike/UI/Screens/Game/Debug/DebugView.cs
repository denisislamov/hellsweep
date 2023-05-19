using TMPro;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
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
		
		[SerializeField] 
		private ButtonView _startFirstBossButtonView;
		
		[SerializeField] 
		private ButtonView _startSecondBossButtonView;
		
		[SerializeField] 
		private ButtonView _startThirdBossButtonView;
		
		[SerializeField] 
		private ButtonView _winButtonView;
		
		[SerializeField] 
		private ButtonView _loseButtonView;

		public IButtonView StartFirstBossButtonView => _startFirstBossButtonView;
		
		public IButtonView StartSecondBossButtonView => _startSecondBossButtonView;
		
		public IButtonView StartThirdBossButtonView => _startThirdBossButtonView;
		
		public IButtonView WinButtonView => _winButtonView;
		
		public IButtonView LoseButtonView => _loseButtonView;
		
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