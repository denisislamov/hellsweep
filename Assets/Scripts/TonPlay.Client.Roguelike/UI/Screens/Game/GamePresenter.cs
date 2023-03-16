using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	internal class GamePresenter : Presenter<IGameView, IGameScreenContext>
	{
		private readonly LevelProgressBarPresenter.Factory _levelProgressBarPresenterFactory;
		private readonly PlayerHealthBarPresenter.Factory _playerHealthBarPresenterFactory;
		private readonly BossHealthBarPresenter.Factory _bossHealthBarPresenterFactory;
		private readonly MatchScorePresenter.Factory _matchScorePresenter;
		private readonly TimerPresenter.Factory _timerPresenterFactory;
		private readonly DebugPresenter.Factory _debugPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;
		
		private IDisposable _bossExistsListener;
		private IPresenter _bossPresenter;

		public GamePresenter(
			IGameView view,
			IGameScreenContext context,
			LevelProgressBarPresenter.Factory levelProgressBarPresenterFactory,
			PlayerHealthBarPresenter.Factory playerHealthBarPresenterFactory,
			BossHealthBarPresenter.Factory bossHealthBarPresenterFactory,
			MatchScorePresenter.Factory matchScorePresenter,
			TimerPresenter.Factory timerPresenterFactory,
			DebugPresenter.Factory debugPresenterFactory,
			IGameModelProvider gameModelProvider)
			: base(view, context)
		{
			_levelProgressBarPresenterFactory = levelProgressBarPresenterFactory;
			_playerHealthBarPresenterFactory = playerHealthBarPresenterFactory;
			_bossHealthBarPresenterFactory = bossHealthBarPresenterFactory;
			_matchScorePresenter = matchScorePresenter;
			_timerPresenterFactory = timerPresenterFactory;
			_debugPresenterFactory = debugPresenterFactory;
			_gameModelProvider = gameModelProvider;

			AddHealthBarPresenter();
			AddExperienceBarPresenter();
			AddMatchScorePresenter();
			AddTimerPresenter();
			AddDebugPresenter();
			AddBossHealthBarPresenter();
		}
		
		private void AddBossHealthBarPresenter()
		{
			var presenter = _bossHealthBarPresenterFactory.Create(
				View.BossHealthProgressBarView,
				ScreenContext.Empty);

			Presenters.Add(presenter);
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
			var presenter = _playerHealthBarPresenterFactory.Create(
				View.HealthProgressBarView,
				new ProgressBarContext(playerModel.Health, playerModel.MaxHealth));

			Presenters.Add(presenter);
		}

		private void AddExperienceBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;
			var presenter = _levelProgressBarPresenterFactory.Create(
				View.LevelProgressBarView,
				new LevelProgressBarContext(playerModel.SkillsModel.Level, playerModel.Experience, playerModel.MaxExperience));

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