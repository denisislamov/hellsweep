using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	internal class GamePresenter : Presenter<IGameView, IGameScreenContext>
	{
		private readonly ProgressBarPresenter.Factory _progressBarPresenterFactory;
		private readonly MatchScorePresenter.Factory _matchScorePresenter;
		private readonly TimerPresenter.Factory _timerPresenterFactory;
		private readonly DebugPresenter.Factory _debugPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;

		public GamePresenter(
			IGameView view, 
			IGameScreenContext context,
			ProgressBarPresenter.Factory progressBarPresenterFactory,
			MatchScorePresenter.Factory matchScorePresenter,
			TimerPresenter.Factory timerPresenterFactory,
			DebugPresenter.Factory debugPresenterFactory,
			IGameModelProvider gameModelProvider) 
			: base(view, context)
		{
			_progressBarPresenterFactory = progressBarPresenterFactory;
			_matchScorePresenter = matchScorePresenter;
			_timerPresenterFactory = timerPresenterFactory;
			_debugPresenterFactory = debugPresenterFactory;
			_gameModelProvider = gameModelProvider;

			AddHealthBarPresenter();
			AddExperienceBarPresenter();
			AddMatchScorePresenter();
			AddTimerPresenter();
			AddDebugPresenter();
		}
		
		private void AddMatchScorePresenter()
		{
			var gameModel = _gameModelProvider.Get();
			var playerModel = gameModel.PlayerModel;
			var presenter = _matchScorePresenter.Create(
				View.MatchScoreView, 
				new MatchScoreContext(playerModel.MatchProfileGainModel.Gold, gameModel.DeadEnemiesCount));
			
			Presenters.Add(presenter);
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
		
		private void AddDebugPresenter()
		{
			var presenter = _debugPresenterFactory.Create(
				View.DebugView, 
				ScreenContext.Empty);
			
			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IGameView, IGameScreenContext, GamePresenter>
		{
		}
	}
}