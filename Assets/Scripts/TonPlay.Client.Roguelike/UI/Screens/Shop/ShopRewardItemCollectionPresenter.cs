using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	internal class ShopRewardItemCollectionPresenter : CollectionPresenter<IShopRewardItemView, IShopRewardItemCollectionContext>
	{
		private readonly ShopRewardItemPresenter.Factory _itemPresenterFactory;
		public ShopRewardItemCollectionPresenter(
			ICollectionView<IShopRewardItemView> view,
			IShopRewardItemCollectionContext screenContext,
			ICollectionItemPool<IShopRewardItemView> itemPool,
			ShopRewardItemPresenter.Factory itemPresenterFactory)
			: base(view, screenContext, itemPool)
		{
			_itemPresenterFactory = itemPresenterFactory;
			AddPresenters();
		}
		
		private void AddPresenters()
		{
			foreach (var context in Context.Rewards)
			{
				var view = Add();
				var presenter = _itemPresenterFactory.Create(view, context);

				Presenters.Add(presenter);
			}
			
			View.RebuildLayout();
		}

		internal class Factory : PlaceholderFactory<IShopRewardItemCollectionView, IShopRewardItemCollectionContext, ShopRewardItemCollectionPresenter>
		{
		}
	}
}