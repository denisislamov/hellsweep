using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar
{
	internal class ProgressBarPresenter : Presenter<IProgressBarView, IProgressBarContext>
	{
		private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

		public ProgressBarPresenter(IProgressBarView view, IProgressBarContext context) : base(view, context)
		{
			InitView();
			AddSubscription();
		}

		public override void Dispose()
		{
			_compositeDisposable?.Dispose();
			base.Dispose();
		}

		private void InitView()
		{
			ChangeListener(Context.CurrentValue.Value/Context.MaxValue.Value);
		}

		private void AddSubscription()
		{
			Context.CurrentValue.Subscribe(ChangeListener).AddTo(_compositeDisposable);
			Context.MaxValue.Subscribe(ChangeListener).AddTo(_compositeDisposable);
		}

		private void ChangeListener(float value)
		{
			if (Context.MaxValue.Value == 0) return;
			
			var size = Context.CurrentValue.Value/Context.MaxValue.Value;
			View.SetSize(size);
		}

		internal class Factory : PlaceholderFactory<IProgressBarView, IProgressBarContext, ProgressBarPresenter>
		{
		}
	}
}