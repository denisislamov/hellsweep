using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackItemCollectionContext : IScreenContext
	{
		IShopPackRewardsModel RewardsModel { get; }
	}
}