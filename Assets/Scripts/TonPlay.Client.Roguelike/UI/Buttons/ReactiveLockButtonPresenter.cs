using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ReactiveLockButtonPresenter : Presenter<IButtonView, IReactiveLockButtonContext>, IButtonPresenter
	{
		private IDisposable _subscription;
		public ReactiveLockButtonPresenter(
			IButtonView view,
			IReactiveLockButtonContext context) : base(view, context)
		{
			AddSubscription();
		}

		public override void Dispose()
		{
			_subscription?.Dispose();

			base.Dispose();
		}

		private void AddSubscription()
		{
			_subscription = Context.Locked.Subscribe(text => ChangeViewLockState());
		}

		private void ChangeViewLockState()
		{
			View.SetLockState(Context.Locked.Value);
		}

		public class Factory : PlaceholderFactory<IButtonView, IReactiveLockButtonContext, ReactiveLockButtonPresenter>
		{
		}
	}
}