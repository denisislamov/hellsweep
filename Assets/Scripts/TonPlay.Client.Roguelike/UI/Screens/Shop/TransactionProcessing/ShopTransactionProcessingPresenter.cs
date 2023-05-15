using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
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
		private readonly IUIService _uiService;
		
		private readonly ReactiveProperty<bool> _closeButtonLocked = new ReactiveProperty<bool>();
		
		public ShopTransactionProcessingPresenter(
			IShopTransactionProcessingView view, 
			IShopTransactionProcessingScreenContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			IUIService uiService) 
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_uiService = uiService;

			AddPayButtonPresenter();
			AddCloseButtonPresenter();
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
				   .Add(new ClickableButtonContext(PayButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void PayButtonClickHandler()
		{
			_closeButtonLocked.SetValueAndForceNotify(true);
			
			Application.OpenURL(Context.TonkeeperUrl);
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