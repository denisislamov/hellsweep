using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	internal class ShopLootboxesPresenter : Presenter<IShopLootboxesView, IShopLootboxesScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ShopLootboxPresenter.Factory _shopLootboxPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		public ShopLootboxesPresenter(
			IShopLootboxesView view,
			IShopLootboxesScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			ShopLootboxPresenter.Factory shopLootboxPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_shopLootboxPresenterFactory = shopLootboxPresenterFactory;

			AddLootboxPresenter(View.CommonLootboxView, RarityName.COMMON);
			AddLootboxPresenter(View.UncommonLootboxView, RarityName.UNCOMMON);
			AddLootboxPresenter(View.RareLootboxView, RarityName.RARE);
			AddLootboxPresenter(View.LegendaryLootboxView, RarityName.LEGENDARY);
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
		
		private void AddLootboxPresenter(IShopLootboxView view, RarityName rarityName)
		{
			var presenter = _shopLootboxPresenterFactory.Create(view, new ShopLootboxContext(rarityName){ Screen = Context.Screen });
			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IShopLootboxesView, IShopLootboxesScreenContext, ShopLootboxesPresenter>
		{
		}
	}
}