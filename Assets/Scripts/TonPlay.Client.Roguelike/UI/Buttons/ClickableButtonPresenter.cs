using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ClickableButtonPresenter : Presenter<IButtonView, IClickableButtonContext>, IButtonPresenter
	{
		private IDisposable _subscription;
		public ClickableButtonPresenter(
			IButtonView view, 
			IClickableButtonContext context) : base(view, context)
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
			_subscription = View.OnClick.Subscribe(unit => Context.OnClick?.Invoke());
		}

		public class Factory : PlaceholderFactory<IButtonView, IClickableButtonContext, ClickableButtonPresenter>
		{
		}
	}
}