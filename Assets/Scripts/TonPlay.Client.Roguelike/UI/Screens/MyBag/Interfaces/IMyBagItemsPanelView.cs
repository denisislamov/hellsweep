using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces
{
	public interface IMyBagItemsPanelView : IView
	{
		IShopRewardItemCollectionView ItemsCollectionView { get; }
	}
}