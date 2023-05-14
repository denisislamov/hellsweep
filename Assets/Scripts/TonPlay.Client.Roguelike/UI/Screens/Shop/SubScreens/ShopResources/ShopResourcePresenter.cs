using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	internal class ShopResourcePresenter : Presenter<IShopResourceView, IShopResourceContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		
		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		public ShopResourcePresenter(
			IShopResourceView view,
			IShopResourceContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;

			InitView();
			AddButtonPresenter();
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
			View.SetIcon(Context.Icon);
			View.SetTitleText(Context.Title);
			View.SetAmountText($"x{Context.Model.Amount.ConvertToSuffixedFormat(1000, 2)}");
			View.SetBackgroundGradientMaterial(Context.Gradient);
		}
		
		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void OnButtonClickHandler()
		{
			IShopResourcePopupScreenContext context = new ShopResourcePopupScreenContext(
				Context.Model,
				Context.Title,
				Context.Icon);

			if (Context is IShopItemResourceContext itemResourceContext)
			{
				context = new ShopItemResourcePopupScreenContext(itemResourceContext.ItemDetailId, context);
			}

			var screen = _uiService.Open<ShopResourcePopupScreen, IShopResourcePopupScreenContext>(context);

			context.Screen = screen;
		}

		internal class Factory : PlaceholderFactory<IShopResourceView, IShopResourceContext, ShopResourcePresenter>
		{
		}
	}
}