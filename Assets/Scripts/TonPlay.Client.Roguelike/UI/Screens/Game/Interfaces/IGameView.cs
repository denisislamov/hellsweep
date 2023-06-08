using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces
{
	public interface IGameView : IView
	{
		IPlayerHealthBarView HealthProgressBarView { get; }
		
		IProgressBarView BossHealthProgressBarView { get; }

		ILevelProgressBarView LevelProgressBarView { get; }

		ITimerView TimerView { get; }

		IMatchScoreView MatchScoreView { get; }
		
		IButtonView PauseButtonView { get; }

		IDebugView DebugView { get; }
	}
}