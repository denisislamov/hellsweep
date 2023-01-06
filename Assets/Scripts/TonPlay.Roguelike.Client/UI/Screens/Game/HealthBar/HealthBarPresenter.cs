using System;
using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.HealthBar
{
	internal class HealthBarPresenter : Presenter<IHealthBarView, IHealthBarContext>
	{
		private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
		
		public HealthBarPresenter(IHealthBarView view, IHealthBarContext context) : base(view, context)
		{
			AddSubscription();
		}

		public override void Dispose()
		{
			_compositeDisposable?.Dispose();
			base.Dispose();
		}

		private void AddSubscription()
		{
			Context.CurrentHealth.Subscribe(HealthChangeListener).AddTo(_compositeDisposable);
			Context.MaxHealth.Subscribe(HealthChangeListener).AddTo(_compositeDisposable);
		}

		private void HealthChangeListener(int currentHealth)
		{
			View.SetSize((float) currentHealth / (float) Context.MaxHealth.Value);
		}

		internal class Factory : PlaceholderFactory<IHealthBarView, IHealthBarContext, HealthBarPresenter>
		{
		}
	}
}