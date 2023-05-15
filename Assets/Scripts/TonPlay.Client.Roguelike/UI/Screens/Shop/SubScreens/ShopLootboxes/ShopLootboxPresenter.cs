using System.Collections.Generic;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	internal class ShopLootboxPresenter : Presenter<IShopLootboxView, IShopLootboxContext>
	{
		private readonly IUIService _uiService;
		private readonly IRestApiClient _restApiClient;
		private readonly IShopEmbeddedScreenStorage _embeddedScreenStorage;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private readonly ReactiveProperty<bool> _buttonLockReactiveProperty = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<string> _buttonTextReactiveProperty = new ReactiveProperty<string>();

		public ShopLootboxPresenter(
			IShopLootboxView view,
			IShopLootboxContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			IShopEmbeddedScreenStorage embeddedScreenStorage)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_embeddedScreenStorage = embeddedScreenStorage;

			AddButtonPresenter();
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
			var metaGameModel = _metaGameModelProvider.Get();
			var keysCount = metaGameModel.ProfileModel.InventoryModel.ToData().GetKeysValue(Context.Rarity);
			_buttonLockReactiveProperty.SetValueAndForceNotify(keysCount < 1);
			//_buttonLockReactiveProperty.SetValueAndForceNotify(false);
			_buttonTextReactiveProperty.SetValueAndForceNotify($"{keysCount}/1 {Context.Rarity.ToString().ToLowerInvariant().FirstCharToUpper()} Keys");
		}

		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_buttonLockReactiveProperty))
				   .Add(new ReactiveTextButtonContext(_buttonTextReactiveProperty)));

			Presenters.Add(presenter);
		}

		private async void ButtonClickHandler()
		{
			var response = await _restApiClient.PostItemLoot(Context.Rarity);

			if (!response.successful)
			{
				return;
			}

			var itemModel = new InventoryItemModel();
			var itemData = new InventoryItemData()
			{
				Id = response.response.id,
				DetailId = response.response.itemDetailId,
				ItemId = response.response.itemId
			};
			itemModel.Update(itemData);

			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();
			inventoryData.Items.Add(itemData);
			inventoryModel.Update(inventoryData);

			if (_embeddedScreenStorage.Current != null)
			{
				_uiService.Close(_embeddedScreenStorage.Current, true);
			}

			_embeddedScreenStorage.Set(
				_uiService.Open<ShopLootboxOpeningScreen, IShopLootboxOpeningScreenContext>(
					new ShopLootboxOpeningScreenContext(new List<IInventoryItemModel>()
					{
						itemModel
					}),
					true));
		}

		internal class Factory : PlaceholderFactory<IShopLootboxView, IShopLootboxContext, ShopLootboxPresenter>
		{
		}
	}
}