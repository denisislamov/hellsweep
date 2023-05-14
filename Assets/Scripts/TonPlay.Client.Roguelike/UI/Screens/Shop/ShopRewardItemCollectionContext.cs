using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopRewardItemCollectionContext : ScreenContext, IShopRewardItemCollectionContext
	{
		public IReadOnlyList<IShopRewardItemContext> Rewards { get; }

		public ShopRewardItemCollectionContext(IReadOnlyList<IShopRewardItemContext> rewards)
		{
			Rewards = rewards;
		}
	}
}