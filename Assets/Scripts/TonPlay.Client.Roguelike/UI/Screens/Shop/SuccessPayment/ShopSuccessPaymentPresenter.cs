using System.Threading;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment
{
	internal class ShopSuccessPaymentPresenter : Presenter<IShopSuccessPaymentView, IShopSuccessPaymentScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly ShopRewardItemCollectionPresenter.Factory _collectionPresenterFactory;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly CancellationTokenSource _buyingCancellationTokenSource = new CancellationTokenSource();

		public ShopSuccessPaymentPresenter(
			IShopSuccessPaymentView view,
			IShopSuccessPaymentScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			ShopRewardItemCollectionPresenter.Factory collectionPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_collectionPresenterFactory = collectionPresenterFactory;
			
			AddCollectionPresenter();
			AddBuyButtonPresenter();
			AddCloseButtonPresenter();
		}

		public override void Show()
		{
			base.Show();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			_buyingCancellationTokenSource?.Cancel();
			_buyingCancellationTokenSource?.Dispose();
			base.Dispose();
		}
		
		private void AddCollectionPresenter()
		{
			var presenter = _collectionPresenterFactory.Create(View.ItemCollectionView, new ShopRewardItemCollectionContext(Context.Items));
			
			Presenters.Add(presenter);
		}
		
		private void AddBuyButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.OkButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnOkButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CloseButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnCloseButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void OnCloseButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
		}

		private void OnOkButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
		}

		internal class Factory : PlaceholderFactory<IShopSuccessPaymentView, IShopSuccessPaymentScreenContext, ShopSuccessPaymentPresenter>
		{
		}
	}
}