using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ReactiveTextButtonPresenter : Presenter<IButtonView, IReactiveTextButtonContext>, IButtonPresenter
	{
		private IDisposable _subscription;
		public ReactiveTextButtonPresenter(
			IButtonView view,
			IReactiveTextButtonContext context) : base(view, context)
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
			_subscription = Context.Text.Subscribe(text => ChangeViewText());
		}

		private void ChangeViewText()
		{
			View.SetText(Context.Text.Value);
		}

		public class Factory : PlaceholderFactory<IButtonView, IReactiveTextButtonContext, ReactiveTextButtonPresenter>
		{
		}
	}
}