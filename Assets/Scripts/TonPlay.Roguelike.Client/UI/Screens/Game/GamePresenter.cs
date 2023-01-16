using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.ProgressBar;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	internal class GamePresenter : Presenter<IGameView, IGameScreenContext>
	{
		private readonly ProgressBarPresenter.Factory _progressBarPresenterFactory;
		private readonly TimerPresenter.Factory _timerPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;

		public GamePresenter(
			IGameView view, 
			IGameScreenContext context,
			ProgressBarPresenter.Factory progressBarPresenterFactory,
			TimerPresenter.Factory timerPresenterFactory,
			IGameModelProvider gameModelProvider) 
			: base(view, context)
		{
			_progressBarPresenterFactory = progressBarPresenterFactory;
			_timerPresenterFactory = timerPresenterFactory;
			_gameModelProvider = gameModelProvider;

			AddHealthBarPresenter();
			AddExperienceBarPresenter();
			AddTimerPresenter();
		}
		
		private void AddHealthBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;
			var presenter = _progressBarPresenterFactory.Create(
				View.HealthProgressBarView, 
				new ProgressBarContext(playerModel.Health, playerModel.MaxHealth));
			
			Presenters.Add(presenter);
		}
		
		private void AddExperienceBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;
			var presenter = _progressBarPresenterFactory.Create(
				View.ExperienceProgressBarView, 
				new ProgressBarContext(playerModel.Experience, playerModel.MaxExperience));
			
			Presenters.Add(presenter);
		}
		
		private void AddTimerPresenter()
		{
			var gameModel = _gameModelProvider.Get();
			var presenter = _timerPresenterFactory.Create(
				View.TimerView, 
				new TimerContext(gameModel.GameTime));
			
			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IGameView, IGameScreenContext, GamePresenter>
		{
		}
	}
}