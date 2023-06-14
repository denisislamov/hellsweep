using System.Collections.Generic;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
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
		
		private readonly IAnalyticsServiceWrapper _analyticsServiceWrapper;
		private readonly ILocationConfigStorage _locationConfigStorage;
		
		public ShopLootboxPresenter(
			IShopLootboxView view,
			IShopLootboxContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			IShopEmbeddedScreenStorage embeddedScreenStorage,
			IAnalyticsServiceWrapper analyticsServiceWrapper,
			ILocationConfigStorage locationConfigStorage)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_embeddedScreenStorage = embeddedScreenStorage;
			_analyticsServiceWrapper = analyticsServiceWrapper;
			_locationConfigStorage = locationConfigStorage;
			
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
			if (_metaGameModelProvider.Get().ProfileModel.InventoryModel.ToData().GetKeysValue(Context.Rarity) <= 0)
			{
				return;
			}
			
			if (_embeddedScreenStorage.Current != null)
			{
				_uiService.Close(_embeddedScreenStorage.Current, true);
			}

			var rewards = new Subject<IReadOnlyList<IInventoryItemModel>>();

			_embeddedScreenStorage.Set(
				_uiService.Open<ShopLootboxOpeningScreen, IShopLootboxOpeningScreenContext>(
					new ShopLootboxOpeningScreenContext(rewards),
					true));
			
			var response = await _restApiClient.PostItemLoot(Context.Rarity);

			if (!response.successful)
			{
				_uiService.Close(_embeddedScreenStorage.Current, true);
				_embeddedScreenStorage.Set(
					_uiService.Open<ShopLootboxesScreen, IShopLootboxesScreenContext>(new ShopLootboxesScreenContext(), true));
				return;
			}

			var receivedItemModels = new List<IInventoryItemModel>(response.response.items.Count);
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			var idItems = new List<string>();
			for (var i = 0; i < response.response.items.Count; i++)
			{
				var itemResponseData = response.response.items[i];
				
				var itemModel = new InventoryItemModel();
				var itemData = new InventoryItemData()
				{
					Id = itemResponseData.id,
					DetailId = itemResponseData.itemDetailId,
					ItemId = itemResponseData.itemId
				};
				itemModel.Update(itemData);
				
				inventoryData.Items.Add(itemData);
				
				receivedItemModels.Add(itemModel);
				idItems.Add(itemResponseData.itemId);
			}
			
			var data = _metaGameModelProvider.Get().ProfileModel.ToData();
			
			_analyticsServiceWrapper.OnOpenLootbox(Context.Rarity.ToString(), 
										    string.Join(", ", idItems.ToArray()), 
										    data.BalanceData.Gold, data.Level, 
										    _locationConfigStorage.Current.Value.Id,
										    1);

			inventoryData.SetKeysValue(Context.Rarity, inventoryData.GetKeysValue(Context.Rarity) - 1);
			
			inventoryModel.Update(inventoryData);
			
			rewards.OnNext(receivedItemModels);
		}

		internal class Factory : PlaceholderFactory<IShopLootboxView, IShopLootboxContext, ShopLootboxPresenter>
		{
		}
	}
}