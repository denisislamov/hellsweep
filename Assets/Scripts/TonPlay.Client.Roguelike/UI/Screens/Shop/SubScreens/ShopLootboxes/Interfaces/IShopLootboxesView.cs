using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces
{
	public interface IShopLootboxesView : IView
	{
		IShopLootboxView CommonLootboxView { get; }
		IShopLootboxView UncommonLootboxView { get; }
		IShopLootboxView RareLootboxView { get; }
		IShopLootboxView LegendaryLootboxView { get; }
	}
}