using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	public class GameView : View, IGameView
	{
		[SerializeField]
		private PlayerHealthBarView _healthProgressBarView;

		[SerializeField]
		private ProgressBarView _bossHealthProgressBarView;

		[SerializeField]
		private LevelProgressBarView _levelProgressBarView;

		[SerializeField]
		private TimerView _timerView;

		[SerializeField]
		private MatchScoreView _matchScoreView;

		[SerializeField]
		private ButtonView _pauseButtonView;

		[SerializeField]
		private DebugView _debugView;

		public IPlayerHealthBarView HealthProgressBarView => _healthProgressBarView;
		
		public IProgressBarView BossHealthProgressBarView => _bossHealthProgressBarView;

		public ILevelProgressBarView LevelProgressBarView => _levelProgressBarView;

		public ITimerView TimerView => _timerView;

		public IMatchScoreView MatchScoreView => _matchScoreView;
		
		public IButtonView PauseButtonView => _pauseButtonView;

		public IDebugView DebugView => _debugView;
	}
}