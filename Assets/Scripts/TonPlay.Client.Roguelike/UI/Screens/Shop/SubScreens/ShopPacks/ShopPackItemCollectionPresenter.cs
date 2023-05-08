using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	internal class ShopPackItemCollectionPresenter : CollectionPresenter<IShopPackItemView, IShopPackItemCollectionContext>
	{
		private readonly ShopPackItemPresenter.Factory _itemPresenterFactory;
		private readonly IShopPackPresentationProvider _shopPackPresentationProvider;

		public ShopPackItemCollectionPresenter(
			ICollectionView<IShopPackItemView> view,
			IShopPackItemCollectionContext screenContext,
			ICollectionItemPool<IShopPackItemView> itemPool,
			ShopPackItemPresenter.Factory itemPresenterFactory,
			IShopPackPresentationProvider shopPackPresentationProvider)
			: base(view, screenContext, itemPool)
		{
			_itemPresenterFactory = itemPresenterFactory;
			_shopPackPresentationProvider = shopPackPresentationProvider;

			InitView();
		}

		private void InitView()
		{
			if (Context.RewardsModel.Coins > 0)
			{
				AddRewardPresenter("coins", Context.RewardsModel.Coins);
			}
			
			if (Context.RewardsModel.Energy > 0)
			{
				AddRewardPresenter("energy", Context.RewardsModel.Energy);
			}
			
			if (Context.RewardsModel.Blueprints > 0)
			{
				AddRewardPresenter("blueprints", Context.RewardsModel.Blueprints);
			}
			
			if (Context.RewardsModel.HeroSkins > 0)
			{
				AddRewardPresenter("hero_skins", Context.RewardsModel.HeroSkins);
			}
			
			if (Context.RewardsModel.KeysCommon > 0)
			{
				AddRewardPresenter("keys_common", Context.RewardsModel.KeysCommon);
			}
			
			if (Context.RewardsModel.KeysUncommon > 0)
			{
				AddRewardPresenter("keys_uncommon", Context.RewardsModel.KeysUncommon);
			}
			
			if (Context.RewardsModel.KeysRare > 0)
			{
				AddRewardPresenter("keys_rare", Context.RewardsModel.KeysRare);
			}
			
			if (Context.RewardsModel.KeysLegendary > 0)
			{
				AddRewardPresenter("keys_legendary", Context.RewardsModel.KeysLegendary);
			}
		}
		
		private void AddRewardPresenter(string rewardId, ulong amount)
		{
			var presentation = _shopPackPresentationProvider.GetRewardPresentation(rewardId);

			var view = Add();
			var context = new ShopPackItemContext($"x{amount.ConvertToSuffixedFormat(1000, 2)}", presentation.Icon, presentation.BackgroundGradientMaterial);
			var presenter = _itemPresenterFactory.Create(view, context);

			Presenters.Add(presenter);
		}

		internal class Factory : PlaceholderFactory<IShopPackItemCollectionView, IShopPackItemCollectionContext, ShopPackItemCollectionPresenter>
		{
		}
	}
}