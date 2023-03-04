using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemCollectionPresenter : CollectionPresenter<IRewardItemView, IRewardItemCollectionContext>
	{
		private readonly IRewardPresentationProvider _rewardPresentationProvider;
		private readonly RewardItemPresenter.Factory _rewardItemPresenterFactory;

		public RewardItemCollectionPresenter(
			ICollectionView<IRewardItemView> view, 
			IRewardItemCollectionContext screenContext, 
			ICollectionItemPool<IRewardItemView> itemPool,
			IRewardPresentationProvider rewardPresentationProvider,
			RewardItemPresenter.Factory rewardItemPresenterFactory) 
			: base(view, screenContext, itemPool)
		{
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

				var itemView = Add();
				var presenter = _rewardItemPresenterFactory.Create(
					itemView,
					new RewardItemContext(presentation.Icon, reward.Count));
				
				Presenters.Add(presenter);
			}
		}
		
		public class Factory : PlaceholderFactory<IRewardItemCollectionView, IRewardItemCollectionContext, RewardItemCollectionPresenter>
		{
		}
	}
}