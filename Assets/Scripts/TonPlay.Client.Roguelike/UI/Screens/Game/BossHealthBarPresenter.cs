using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game
{
	internal class BossHealthBarPresenter : Presenter<IProgressBarView, IScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly ProgressBarPresenter.Factory _basePresenterFactory;
		
		private IDisposable _bossExistsListener;

		public BossHealthBarPresenter(
			IProgressBarView view, 
			IScreenContext context,
			IGameModelProvider gameModelProvider,
			ProgressBarPresenter.Factory basePresenterFactory) : base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_basePresenterFactory = basePresenterFactory;

			AddBossHealthBarPresenter();
			AddBossExistsSubscription();
		}

		public override void Show()
		{
			base.Show();
			
			var bossModel = _gameModelProvider.Get().BossModel;

			if (bossModel.Exists.Value) 
			{
				View.Show();
			}
			else
			{
				View.Hide();
			}
		}

		public override void Dispose()
		{
			_bossExistsListener?.Dispose();
			
			base.Dispose();
		}

		private void AddBossExistsSubscription()
		{
			var bossModel = _gameModelProvider.Get().BossModel;
			_bossExistsListener = bossModel.Exists.Subscribe(exists =>
			{
				if (exists)
				{
					View.Show();
				}
				else
				{
					View.Hide();
				}
			});
		}
		
		private void AddBossHealthBarPresenter()
		{
			var bossModel = _gameModelProvider.Get().BossModel;
			var presenter = _basePresenterFactory.Create(
				View,
				new ProgressBarContext(bossModel.Health, bossModel.MaxHealth));

			Presenters.Add(presenter);
		}
		
		internal class Factory : PlaceholderFactory<IProgressBarView, IScreenContext, BossHealthBarPresenter>
		{
		}
	}
}