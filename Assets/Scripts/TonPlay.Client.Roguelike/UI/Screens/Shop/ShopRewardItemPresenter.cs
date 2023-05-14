using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	internal class ShopRewardItemPresenter : Presenter<IShopRewardItemView, IShopRewardItemContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		public ShopRewardItemPresenter(
			IShopRewardItemView view,
			IShopRewardItemContext context,
			IUIService uiService)
			: base(view, context)
		{
			_uiService = uiService;

			InitView();
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
			View.SetAmountText($"{Context.AmountText}");
			View.SetIcon(Context.Icon);
			View.SetBackgroundGradient(Context.BackgroundGradientMaterial);
		}
		
		internal class Factory : PlaceholderFactory<IShopRewardItemView, IShopRewardItemContext, ShopRewardItemPresenter>
		{
		}
	}
}