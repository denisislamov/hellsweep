using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	internal class ShopPackCollectionPresenter : CollectionPresenter<IShopPackView, IShopPackCollectionContext>
	{
		private readonly ShopPackPresenter.Factory _packPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IShopRewardPresentationProvider _shopRewardPresentationProvider;

		public ShopPackCollectionPresenter(
			ICollectionView<IShopPackView> view,
			IShopPackCollectionContext screenContext,
			ICollectionItemPool<IShopPackView> itemPool,
			ShopPackPresenter.Factory packPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IShopRewardPresentationProvider shopRewardPresentationProvider)
			: base(view, screenContext, itemPool)
		{
			_packPresenterFactory = packPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_shopRewardPresentationProvider = shopRewardPresentationProvider;

			InitView();
		}

		private void InitView()
		{
			var packs = _metaGameModelProvider.Get().ShopModel.Packs;

			for (int i = 0; i < packs.Count; i++)
			{
				var packModel = packs[i];
				var packPresentation = _shopRewardPresentationProvider.Get(packModel.Id);
				
				var view = Add();
				var context = new ShopPackContext(packModel, packPresentation.Title, packPresentation.MainColor, packPresentation.BackgroundGradientMaterial);
				var presenter = _packPresenterFactory.Create(view, context);
				
				Presenters.Add(presenter);
			}
		}

		internal class Factory : PlaceholderFactory<IShopPackCollectionView, IShopPackCollectionContext, ShopPackCollectionPresenter>
		{
		}
	}
}