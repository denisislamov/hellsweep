using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	public class GameView : View, IGameView
	{
		[SerializeField]
		private ProgressBarView _healthProgressBarView;
		
		[SerializeField]
		private ProgressBarView _experienceProgressBarView;
		
		[SerializeField]
		private TimerView _timerView;

		public IProgressBarView HealthProgressBarView => _healthProgressBarView;
		
		public IProgressBarView ExperienceProgressBarView => _experienceProgressBarView;
		
		public ITimerView TimerView => _timerView;
	}
}