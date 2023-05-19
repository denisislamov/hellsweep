using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	internal class ShopLootboxItemCollectionPresenter : CollectionPresenter<IShopRewardItemView, IShopLootboxItemCollectionContext>
	{
		private readonly ShopRewardItemPresenter.Factory _itemPresenterFactory;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;

		public ShopLootboxItemCollectionPresenter(
			ICollectionView<IShopRewardItemView> view,
			IShopLootboxItemCollectionContext screenContext,
			ICollectionItemPool<IShopRewardItemView> itemPool,
			ShopRewardItemPresenter.Factory itemPresenterFactory,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, screenContext, itemPool)
		{
			_itemPresenterFactory = itemPresenterFactory;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

			InitView();
		}

		private void InitView()
		{
			foreach (var model in Context.Items)
			{
				AddRewardPresenter(model);
			}
			
			View.RebuildLayout();
		}
		
		private void AddRewardPresenter(IInventoryItemModel model)
		{
			var config = _inventoryItemsConfigProvider.Get(model.ItemId.Value);
			var presentation = _inventoryItemPresentationProvider.GetItemPresentation(model.ItemId.Value);
			
			_inventoryItemPresentationProvider.GetColors(config.Rarity, out var color, out var backgroundGradient);

			if (presentation == null)
			{
				Debug.LogWarning($"[ShopLootboxItemCollectionPresenter] Presentation for reward {model.ItemId.Value} hasn't been found");
				return;
			}

			var view = Add();
			var context = new ShopRewardItemContext(presentation.Title, presentation.Icon,backgroundGradient);
			var presenter = _itemPresenterFactory.Create(view, context);

			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IShopLootboxItemCollectionView, IShopLootboxItemCollectionContext, ShopLootboxItemCollectionPresenter>
		{
		}
	}
}