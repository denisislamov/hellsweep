using System;
using System.Text;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Debug.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Debug
{
	internal class DebugPresenter : Presenter<IDebugView, IScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
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
			IGameModelProvider gameModelProvider)
			: base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_stringBuilder = new StringBuilder();

			AddSubscription();
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

		internal class Factory : PlaceholderFactory<IDebugView, IScreenContext, DebugPresenter>
		{
		}
	}
}