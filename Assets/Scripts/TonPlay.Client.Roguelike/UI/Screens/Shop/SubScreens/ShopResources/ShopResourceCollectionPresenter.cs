using System;
using System.Linq;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	internal class ShopResourceCollectionPresenter : CollectionPresenter<IShopResourceView, IShopResourceCollectionContext>
	{
		private readonly ShopResourcePresenter.Factory _resourcePresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IShopResourcePresentationProvider _shopResourcesPresentationProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;

		public ShopResourceCollectionPresenter(
			ICollectionView<IShopResourceView> view,
			IShopResourceCollectionContext screenContext,
			ICollectionItemPool<IShopResourceView> itemPool,
			ShopResourcePresenter.Factory resourcePresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			IShopResourcePresentationProvider shopResourcesPresentationProvider)
			: base(view, screenContext, itemPool)
		{
			_resourcePresenterFactory = resourcePresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_shopResourcesPresentationProvider = shopResourcesPresentationProvider;

			InitView();
		}

		private void InitView()
		{
			var resources = _metaGameModelProvider.Get().ShopModel.Resources;

			for (int i = 0; i < resources.Count; i++)
			{
				var model = resources[i];
				var presentation = _shopResourcesPresentationProvider.Get(model.Id);

				if (presentation == null)
				{
					Debug.LogWarning($"[ShopResourceCollectionPresenter] Resource presentation for {model.Id} hasn't been found!");
					continue;
				}
				
				if (model.Type == ShopResourceType.Items)
				{
					var itemConfigs = _inventoryItemsConfigProvider.AllConfigs.Where(_ => _.Rarity == model.Rarity).ToList();
					var random = new Random(DateTime.UtcNow.Day);
					var randomItemIdx = random.Next(itemConfigs.Count);
					var randomItemConfig = itemConfigs[randomItemIdx];
					var detailItemConfig = randomItemConfig.GetDetails(1);
					var itemPresentation = _inventoryItemPresentationProvider.GetItemPresentation(randomItemConfig.Id);

					var view = Add();
					var context = new ShopItemResourceContext(
						randomItemConfig.Id, detailItemConfig.Id, model, randomItemConfig.Name, presentation.BackgroundGradientMaterial, itemPresentation.Icon);
					var presenter = _resourcePresenterFactory.Create(view, context);
				
					Presenters.Add(presenter);
				}
				else
				{
					var view = Add();
					var context = new ShopResourceContext(
						model, presentation.Title, presentation.BackgroundGradientMaterial, presentation.Icon);
					var presenter = _resourcePresenterFactory.Create(view, context);
				
					Presenters.Add(presenter);
				}
			}
		}

		internal class Factory : PlaceholderFactory<IShopResourceCollectionView, IShopResourceCollectionContext, ShopResourceCollectionPresenter>
		{
		}
	}
}