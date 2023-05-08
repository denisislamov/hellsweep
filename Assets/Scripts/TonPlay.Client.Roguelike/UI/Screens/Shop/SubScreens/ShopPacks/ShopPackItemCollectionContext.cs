using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackItemCollectionContext : ScreenContext, IShopPackItemCollectionContext
	{
		public IShopPackRewardsModel RewardsModel { get; }

		public ShopPackItemCollectionContext(IShopPackRewardsModel rewardsModel)
		{
			RewardsModel = rewardsModel;
		}
	}
}