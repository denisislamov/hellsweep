using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces
{
	public interface IGameView : IView
	{
		IProgressBarView HealthProgressBarView { get; }
		
		IProgressBarView ExperienceProgressBarView { get; }
		
		ITimerView TimerView { get; }

		IMatchScoreView MatchScoreView { get; }
	}
}