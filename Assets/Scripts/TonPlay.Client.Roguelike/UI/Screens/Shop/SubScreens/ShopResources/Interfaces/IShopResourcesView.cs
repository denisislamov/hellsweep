using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces
{
	public interface IShopResourcesView : IView
	{
		IShopResourceCollectionView CollectionView { get; }
	}
}