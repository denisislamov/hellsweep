using TonPlay.Roguelike.Client.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces
{
	public interface IGameView : IView
	{
		IProgressBarView HealthProgressBarView { get; }
		
		IProgressBarView ExperienceProgressBarView { get; }
		
		ITimerView TimerView { get; }
	}
}