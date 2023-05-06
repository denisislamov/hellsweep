using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemCollectionPresenter : CollectionPresenter<IRewardItemView, IRewardItemCollectionContext>
	{
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IRewardPresentationProvider _rewardPresentationProvider;
		private readonly RewardItemPresenter.Factory _rewardItemPresenterFactory;

		public RewardItemCollectionPresenter(
			ICollectionView<IRewardItemView> view, 
			IRewardItemCollectionContext screenContext, 
			ICollectionItemPool<IRewardItemView> itemPool,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IRewardPresentationProvider rewardPresentationProvider,
			RewardItemPresenter.Factory rewardItemPresenterFactory) 
			: base(view, screenContext, itemPool)
		{
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_rewardPresentationProvider = rewardPresentationProvider;
			_rewardItemPresenterFactory = rewardItemPresenterFactory;

			InitView();
		}
		
		private void InitView()
		{
			for (var i = 0; i < Context.Rewards.Count; i++)
			{
				var reward = Context.Rewards[i];
				var presentation = _rewardPresentationProvider.Get(reward.Id);

				var icon = presentation?.Icon;
				var gradientMaterial = presentation?.BackgroundGradientMaterial;
				if (string.IsNullOrEmpty(presentation?.Id))
				{
					var config = _inventoryItemsConfigProvider.Get(reward.Id);
					
					if (config is null) continue;
					
					var itemPresentation = _inventoryItemPresentationProvider.GetItemPresentation(config.Id);
					_inventoryItemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradient);
					icon = itemPresentation.Icon;
					gradientMaterial = backgroundGradient;
				}

				var itemView = Add();
				var presenter = _rewardItemPresenterFactory.Create(
					itemView,
					new RewardItemContext(icon, gradientMaterial, reward.Count));
				
				Presenters.Add(presenter);
			}
		}
		
		public class Factory : PlaceholderFactory<IRewardItemCollectionView, IRewardItemCollectionContext, RewardItemCollectionPresenter>
		{
		}
	}
}