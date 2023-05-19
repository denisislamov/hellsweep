using System;
using System.Text;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug
{
	internal class DebugPresenter : Presenter<IDebugView, IScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private const double UPDATE_RATE_IN_SECONDS = 0.2f;

		private readonly char[] _chars = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

		private readonly StringBuilder _stringBuilder;

		private IDisposable _updateHandler;
		private IDisposable _timerHandler;
		private IDisposable _debugEnemyMovementHandler;

		private float _deltaCounter;
		private int _frameCounter;

		public DebugPresenter(
			IDebugView view,
			IScreenContext context,
			IGameModelProvider gameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_stringBuilder = new StringBuilder();

#if ENABLE_DEBUG
			AddSubscription();
			AddSetGameTimeToFirstBossButtonPresenter();
			AddSetGameTimeToSecondBossButtonPresenter();
			AddSetGameTimeToThirdBossButtonPresenter();
			AddWinGameButtonPresenter();
			AddLoseButtonPresenter();
#endif
		}

		public override void Show()
		{
			base.Show();
			
#if !ENABLE_DEBUG
			View.Hide();
#endif
		}

		public override void Dispose()
		{
			_updateHandler?.Dispose();
			_timerHandler?.Dispose();
			_debugEnemyMovementHandler?.Dispose();

			base.Dispose();
		}

		private void AddSubscription()
		{
			_updateHandler = Observable.EveryUpdate().Subscribe((unit) => UpdateHandler());
			_debugEnemyMovementHandler = _gameModelProvider.Get().DebugEnemyMovementToEachOtherCollisionCount.Subscribe(UpdateEnemyMovementCollisionCount);
		}
		
		private void UpdateEnemyMovementCollisionCount(int value)
		{
			View.SetEnemyMovementCollisionCount(value);
		}

		private void UpdateHandler()
		{
			if (_deltaCounter < UPDATE_RATE_IN_SECONDS)
			{
				_deltaCounter += Time.deltaTime;
				_frameCounter += 1;
				return;
			}

			_stringBuilder.Clear();

			var framerate = 1f/(_deltaCounter/_frameCounter);

			if (float.IsInfinity(framerate)) return;

			while (framerate >= 1)
			{
				var digit = Mathf.RoundToInt((int)framerate)%10;

				_stringBuilder.Insert(0, _chars[digit]);

				framerate /= 10;
			}

			View.SetFramerateText(_stringBuilder.ToString());

			_deltaCounter = 0f;
			_frameCounter = 0;
		}
		
		private void AddLoseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.LoseButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(LostButtonClickHandler)));
			
			Presenters.Add(presenter);
		}

		private void AddWinGameButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.WinButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(WinButtonClickHandler)));
			
			Presenters.Add(presenter);
		}

		private void AddSetGameTimeToFirstBossButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.StartFirstBossButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(() => SetGameTimeTo(4, 55))));
			
			Presenters.Add(presenter);
		}

		private void AddSetGameTimeToSecondBossButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.StartSecondBossButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(() => SetGameTimeTo(9, 55))));
			
			Presenters.Add(presenter);
		}
		
		private void AddSetGameTimeToThirdBossButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.StartThirdBossButtonView,
				new CompositeButtonContext().Add(new ClickableButtonContext(() => SetGameTimeTo(14, 55))));
			
			Presenters.Add(presenter);
		}
		
		private void SetGameTimeTo(int minutes, int seconds)
		{
			var gameModel = _gameModelProvider.Get();
			var gameData = gameModel.ToData();
			
			gameData.DebugForcedTime = true;
			gameData.GameTimeInSeconds = TimeSpan.FromMinutes(minutes).TotalSeconds + TimeSpan.FromSeconds(seconds).TotalSeconds;
			
			gameModel.Update(gameData);
		}
		
		private void WinButtonClickHandler()
		{
			var gameModel = _gameModelProvider.Get();
			var gameData = gameModel.ToData();
			
			gameData.DebugForcedWin = true;
			gameData.DebugForcedTime = true;
			gameData.GameTimeInSeconds = TimeSpan.FromMinutes(15).TotalSeconds;
			
			gameModel.Update(gameData);
		}
		
		private void LostButtonClickHandler()
		{
			var gameModel = _gameModelProvider.Get();
			var gameData = gameModel.ToData();
			
			gameData.DebugForcedLose = true;
			gameData.PlayerData.Health = 0;
			
			gameModel.Update(gameData);
		}

		internal class Factory : PlaceholderFactory<IDebugView, IScreenContext, DebugPresenter>
		{
		}
	}
}