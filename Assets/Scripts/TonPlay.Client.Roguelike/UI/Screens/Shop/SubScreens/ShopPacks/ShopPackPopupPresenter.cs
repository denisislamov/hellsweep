using System.Threading;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	internal class ShopPackPopupPresenter : Presenter<IShopPackPopupView, IShopPackPopupScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		private readonly IShopRewardPresentationProvider _shopRewardPresentationProvider;
		private readonly ShopPackItemCollectionPresenter.Factory _collectionPresenterFactory;
		private readonly ShopPurchaseAction.Factory _shopPurchaseActionFactory;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly CancellationTokenSource _purchasingCancellationTokenSource = new CancellationTokenSource();

		public ShopPackPopupPresenter(
			IShopPackPopupView view,
			IShopPackPopupScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			IShopRewardPresentationProvider shopRewardPresentationProvider,
			ShopPackItemCollectionPresenter.Factory collectionPresenterFactory,
			ShopPurchaseAction.Factory shopPurchaseActionFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_shopRewardPresentationProvider = shopRewardPresentationProvider;
			_collectionPresenterFactory = collectionPresenterFactory;
			_shopPurchaseActionFactory = shopPurchaseActionFactory;

			InitView();
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
			_purchasingCancellationTokenSource?.Cancel();
			_purchasingCancellationTokenSource?.Dispose();
			base.Dispose();
		}
		
		private void InitView()
		{
			var presentation = _shopRewardPresentationProvider.Get(Context.Model.Id);
			
			View.SetTitleText(presentation.Title);
			View.SetPanelsColor(presentation.MainColor);
			View.SetPriceText($"{Context.Model.Price} TON");
			View.SetRarityText(presentation.RarityText);
			View.SetDescriptionText(presentation.Description);
			View.SetBackgroundGradientMaterial(presentation.BackgroundGradientMaterial);
		}
		
		private void AddCollectionPresenter()
		{
			var presenter = _collectionPresenterFactory.Create(
				View.ItemCollectionView, new ShopPackItemCollectionContext(Context.Model.Rewards));
			
			Presenters.Add(presenter);
		}
		
		private void AddBuyButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.BuyButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnBuyButtonClickHandler)));
			
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

		private async void OnBuyButtonClickHandler()
		{
			var action = _shopPurchaseActionFactory.Create(new ShopPurchaseActionContext(
				PaymentReason.PACK, 
				Context.Model.Id,
				_purchasingCancellationTokenSource.Token));
			
			await action.Begin();

			var result = await action.CompletionSource.Task;
			if (result.Status == PaymentStatus.COMPLETED)
			{
				_uiService.Open<ShopSuccessPaymentScreen, IShopSuccessPaymentScreenContext>(new ShopSuccessPaymentScreenContext(result.Rewards));
			}
		}

		internal class Factory : PlaceholderFactory<IShopPackPopupView, IShopPackPopupScreenContext, ShopPackPopupPresenter>
		{
		}
	}
}