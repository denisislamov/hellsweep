using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar;
using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.Timer;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	internal class GamePresenter : Presenter<IGameView, IGameScreenContext>
	{
		private readonly HealthBarPresenter.Factory _healthBarPresenterFactory;
		private readonly TimerPresenter.Factory _timerPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;

		public GamePresenter(
			IGameView view, 
			IGameScreenContext context,
			HealthBarPresenter.Factory healthBarPresenterFactory,
			TimerPresenter.Factory timerPresenterFactory,
			IGameModelProvider gameModelProvider) 
			: base(view, context)
		{
			_healthBarPresenterFactory = healthBarPresenterFactory;
			_timerPresenterFactory = timerPresenterFactory;
			_gameModelProvider = gameModelProvider;

			AddHealthBarPresenter();
			AddTimerPresenter();
		}
		
		private void AddHealthBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;
			var presenter = _healthBarPresenterFactory.Create(
				View.HealthBarView, 
				new HealthBarContext(playerModel.MaxHealth, playerModel.Health));
			
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