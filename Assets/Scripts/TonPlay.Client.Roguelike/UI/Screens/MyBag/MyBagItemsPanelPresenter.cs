using System;
using System.Collections.Generic;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	internal class MyBagItemsPanelPresenter : Presenter<IMyBagItemsPanelView, IScreenContext>
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IShopRewardPresentationProvider _shopRewardPresentationProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private readonly ShopRewardItemCollectionPresenter.Factory _itemCollectionPresenterFactory;
		
		public MyBagItemsPanelPresenter(
			IMyBagItemsPanelView view, 
			IScreenContext context,
			IMetaGameModelProvider metaGameModelProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IShopRewardPresentationProvider shopRewardPresentationProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			ShopRewardItemCollectionPresenter.Factory itemCollectionPresenterFactory) 
			: base(view, context)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_shopRewardPresentationProvider = shopRewardPresentationProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_itemCollectionPresenterFactory = itemCollectionPresenterFactory;
			
			AddCollectionPresenter();
		}
		
		private void AddCollectionPresenter()
		{
			var model = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			
			var items = new []
			{
				CreateRewardContext("blueprints_arms", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsArms.Value)),
				CreateRewardContext("blueprints_belt", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsBelt.Value)),
				CreateRewardContext("blueprints_body", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsBody.Value)),
				CreateRewardContext("blueprints_feet", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsFeet.Value)),
				CreateRewardContext("blueprints_weapon", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsWeapon.Value)),
				CreateRewardContext("blueprints_neck", RarityName.COMMON, Convert.ToUInt64(model.BlueprintsNeck.Value)),
				CreateRewardContext("keys_common", RarityName.COMMON, Convert.ToUInt64(model.CommonKeys.Value)),
				CreateRewardContext("keys_uncommon", RarityName.UNCOMMON, Convert.ToUInt64(model.UncommonKeys.Value)),
				CreateRewardContext("keys_rare", RarityName.RARE, Convert.ToUInt64(model.RareKeys.Value)),
				CreateRewardContext("keys_legendary", RarityName.LEGENDARY, Convert.ToUInt64(model.LegendaryKeys.Value)),
			};
			
			var presenter = _itemCollectionPresenterFactory.Create(View.ItemsCollectionView, new ShopRewardItemCollectionContext(items));
			
			Presenters.Add(presenter);
		}

		private IShopRewardItemContext CreateRewardContext(string rewardId, RarityName rarityName, ulong amount)
		{
			var presentation = _shopRewardPresentationProvider.GetRewardPresentation(rewardId);

			Color color;
			Material gradient;
			
			if (presentation == null)
			{
				var config = _inventoryItemsConfigProvider.GetConfigByDetailId(rewardId);

				if (config == null)
				{
					Debug.LogWarning($"[ShopPurchaseAction] Presentation for reward {rewardId} hasn't been found");
					return null;
				}

				var itemPresentation = _inventoryItemPresentationProvider.GetItemPresentation(config.Id);

				if (itemPresentation == null)
				{
					Debug.LogWarning($"[ShopPurchaseAction] Presentation for reward {rewardId} hasn't been found");
					return null;
				}

				_inventoryItemPresentationProvider.GetColors(config.Rarity, out color, out gradient);

				return new ShopRewardItemWithTextPanelContext(itemPresentation.Title, itemPresentation.Icon, gradient, color);
			}
			
			_inventoryItemPresentationProvider.GetColors(rarityName, out color, out gradient);

			return new ShopRewardItemWithTextPanelContext($"x{amount.ConvertToSuffixedFormat(1000, 2)}", presentation.Icon, presentation.BackgroundGradientMaterial, color);
		}
		
		public class Factory : PlaceholderFactory<IMyBagItemsPanelView, IScreenContext, MyBagItemsPanelPresenter>
		{
		}
	}
}