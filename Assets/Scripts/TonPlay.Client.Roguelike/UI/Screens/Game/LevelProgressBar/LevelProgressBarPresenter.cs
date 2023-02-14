using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar
{
	internal class LevelProgressBarPresenter : Presenter<ILevelProgressBarView, ILevelProgressBarContext>
	{
		private readonly ProgressBarPresenter.Factory _progressBarPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;
		
		private IDisposable _subscription;

		public LevelProgressBarPresenter(
			ILevelProgressBarView view, 
			ILevelProgressBarContext context,
			ProgressBarPresenter.Factory progressBarPresenterFactory) 
			: base(view, context)
		{
			_progressBarPresenterFactory = progressBarPresenterFactory;

			UpdateLevelView();
			AddLevelSubscription();
			AddNestedProgressBarPresenter();
		}

		public override void Dispose()
		{
			_subscription?.Dispose();
			base.Dispose();
		}

		private void UpdateLevelView()
		{
			View.SetLevelText(Context.Level.Value.ToString());
		}
		
		private void AddLevelSubscription()
		{
			_subscription?.Dispose();
			
			_subscription = Context.Level.Subscribe(level => UpdateLevelView());
		}

		private void AddNestedProgressBarPresenter()
		{
			var presenter = _progressBarPresenterFactory.Create(View, 
				new ProgressBarContext(Context.Value, Context.MaxValue));
			
			Presenters.Add(presenter);
		}
		
		internal class Factory : PlaceholderFactory<ILevelProgressBarView, ILevelProgressBarContext, LevelProgressBarPresenter>
		{
		}
	}
}