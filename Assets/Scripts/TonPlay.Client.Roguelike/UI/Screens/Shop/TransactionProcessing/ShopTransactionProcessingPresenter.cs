using System;
using ReactWrapper.TelegramAPI;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingPresenter : Presenter<IShopTransactionProcessingView, IShopTransactionProcessingScreenContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ITelegramPlatformProvider _telegramPlatformProvider;

		private readonly ReactiveProperty<bool> _closeButtonLocked = new ReactiveProperty<bool>(true);
		private readonly ReactiveProperty<bool> _payButtonLocked = new ReactiveProperty<bool>(true);
		
		private IDisposable _tonkeeperSubscription;
		private string _tonkeeperUrl;
		private IDisposable _receiveResponseSubscription;

		public ShopTransactionProcessingPresenter(
			IShopTransactionProcessingView view, 
			IShopTransactionProcessingScreenContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			ITelegramPlatformProvider telegramPlatformProvider)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_telegramPlatformProvider = telegramPlatformProvider;

			AddReceiveResponseSubscription();
			AddTonkeeperUrlSubscription();
			AddPayButtonPresenter();
			AddCloseButtonPresenter();
		}

		public override void Dispose()
		{
			_tonkeeperSubscription?.Dispose();
			_receiveResponseSubscription?.Dispose();
			base.Dispose();
		}
		
		private void AddReceiveResponseSubscription()
		{
			_receiveResponseSubscription = Context.ResponseReceived.Subscribe(received =>
			{
				if (!received)
				{
					return;
				}
				
				_receiveResponseSubscription?.Dispose();
				_closeButtonLocked.SetValueAndForceNotify(false);
			});
		}

		private void AddTonkeeperUrlSubscription()
		{
			_tonkeeperSubscription = Context.TonkeeperUrl.Subscribe(url =>
			{
				if (string.IsNullOrEmpty(url) || !string.IsNullOrEmpty(_tonkeeperUrl))
				{
					return;
				}

				_tonkeeperSubscription?.Dispose();
				
				_tonkeeperUrl = url;
				_payButtonLocked.SetValueAndForceNotify(false);
			});
		}

		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CancelButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(CloseButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_closeButtonLocked)));
			
			Presenters.Add(presenter);
		}

		private void AddPayButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.PayButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(PayButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_payButtonLocked)));

			Presenters.Add(presenter);
		}
		
		private void PayButtonClickHandler()
		{
			_closeButtonLocked.SetValueAndForceNotify(true);

			if (_telegramPlatformProvider.Current == TelegramPlatform.Desktop || _telegramPlatformProvider.Current == TelegramPlatform.MacOS)
			{
				TelegramAPI.OpenLink($"https://chart.googleapis.com/chart?cht=qr&chs=300x300&chl={Context.TonkeeperUrl.Value}");
			}
			else
			{
				TelegramAPI.OpenLink(Context.TonkeeperUrl.Value);
			}
		}
		
		private void CloseButtonClickHandler()
		{
			Context.CloseButtonClickCallback?.Invoke();
		}

		public class Factory : PlaceholderFactory<IShopTransactionProcessingView, IShopTransactionProcessingScreenContext, ShopTransactionProcessingPresenter>
		{
		}
	}
}