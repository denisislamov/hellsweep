using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	internal class ShopResourcePopupPresenter : Presenter<IShopResourcePopupView, IShopResourcePopupScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		public ShopResourcePopupPresenter(
			IShopResourcePopupView view,
			IShopResourcePopupScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

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

		private void OnBuyButtonClickHandler()
		{
			Debug.Log($"Clicked shop resource with id {Context.Model.Id}");
		}

		internal class Factory : PlaceholderFactory<IShopResourcePopupView, IShopResourcePopupScreenContext, ShopResourcePopupPresenter>
		{
		}
	}
}