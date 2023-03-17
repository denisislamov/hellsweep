using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;
using Screen = UnityEngine.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	internal class PlayerHealthBarPresenter : Presenter<IPlayerHealthBarView, IScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly ProgressBarPresenter.Factory _basePresenterFactory;
		
		private Camera _camera;

		private IDisposable _positionListener;

		public PlayerHealthBarPresenter(
			IPlayerHealthBarView view, 
			IScreenContext context,
			IGameModelProvider gameModelProvider,
			ProgressBarPresenter.Factory basePresenterFactory) : base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_basePresenterFactory = basePresenterFactory;

			AddProgressBarPresenter();
			AddPositionSubscription();
		}

		public override void Dispose()
		{
			_positionListener?.Dispose();
			
			base.Dispose();
		}

		private void AddPositionSubscription()
		{
			_positionListener = Observable.EveryUpdate().Subscribe(position =>
			{
				var playerModel = _gameModelProvider.Get().PlayerModel;

				if (_camera == null)
				{
					_camera = Camera.main;
				}

				if (_camera == null) return;
				
				var screenPosition = _camera.WorldToScreenPoint(
					playerModel.Position.Value + 
					RoguelikeConstants.Core.UI.HEALTH_OFFSET);

				View.SetPosition(screenPosition);
			});
		}
		
		private void AddProgressBarPresenter()
		{
			var playerModel = _gameModelProvider.Get().PlayerModel;
			var presenter = _basePresenterFactory.Create(
				View,
				new ProgressBarContext(playerModel.Health, playerModel.MaxHealth));

			Presenters.Add(presenter);
		}
		
		internal class Factory : PlaceholderFactory<IPlayerHealthBarView, IScreenContext, PlayerHealthBarPresenter>
		{
		}
	}
}