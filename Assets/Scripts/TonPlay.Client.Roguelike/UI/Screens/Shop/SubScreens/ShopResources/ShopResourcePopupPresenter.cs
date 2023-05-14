using System;
using System.Threading;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	internal class ShopResourcePopupPresenter : Presenter<IShopResourcePopupView, IShopResourcePopupScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private readonly ShopPurchaseAction.Factory _shopPurchaseActionFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly CancellationTokenSource _purchasingCancellationTokenSource = new CancellationTokenSource();

		public ShopResourcePopupPresenter(
			IShopResourcePopupView view,
			IShopResourcePopupScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			ShopPurchaseAction.Factory shopPurchaseActionFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_shopPurchaseActionFactory = shopPurchaseActionFactory;

			InitView();
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
			base.Dispose();
		}
		
		private void InitView()
		{
			_inventoryItemPresentationProvider.GetColors(Context.Model.Rarity, out var mainColor, out var backgroundGradient);
			View.SetIcon(Context.Icon);
			View.SetTitleText(Context.Title);
			View.SetPanelsColor(mainColor);
			View.SetPriceText($"{Context.Model.Price} TON");
			View.SetRarityText(Context.Model.Rarity.ToString().ToLowerInvariant().FirstCharToUpper());
			View.SetAmountText($"x{Context.Model.Amount.ConvertToSuffixedFormat(1000, 2)}");
			View.SetBackgroundGradientMaterial(backgroundGradient);
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
			var reason = PaymentReason.ITEM;
			object value = null;
			switch (Context.Model.Type)
			{
				case ShopResourceType.Items:
				{
					reason = PaymentReason.ITEM;
					value = ((IShopItemResourcePopupScreenContext) Context).ItemDetailId;
					break;
				}
				case ShopResourceType.Keys:
					reason = PaymentReason.KEYS;
					value = Context.Model.Rarity;
					break;
				case ShopResourceType.Energy:
					reason = PaymentReason.ENERGY;
					break;
				case ShopResourceType.Blueprints:
					reason = PaymentReason.BLUEPRINTS;
					break;
				case ShopResourceType.Coins:
					throw new NotImplementedException();
				case ShopResourceType.HeroSkins:
					throw new NotImplementedException();
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			var action = _shopPurchaseActionFactory.Create(
				new ShopPurchaseActionContext(reason, value, _purchasingCancellationTokenSource.Token));
			
			await action.Begin();

			var result = await action.CompletionSource.Task;
			if (result.Status == PaymentStatus.COMPLETED)
			{
				_uiService.Open<ShopSuccessPaymentScreen, IShopSuccessPaymentScreenContext>(new ShopSuccessPaymentScreenContext(result.Rewards));
			}
		}

		internal class Factory : PlaceholderFactory<IShopResourcePopupView, IShopResourcePopupScreenContext, ShopResourcePopupPresenter>
		{
		}
	}
}