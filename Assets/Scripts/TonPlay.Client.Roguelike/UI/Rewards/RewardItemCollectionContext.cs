using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemCollectionContext : ScreenContext, IRewardItemCollectionContext
	{
		public IReadOnlyList<IRewardData> Rewards { get; }
		
		public RewardItemCollectionContext(IReadOnlyList<IRewardData> rewards)
		{
			Rewards = rewards;
		}
	}
}