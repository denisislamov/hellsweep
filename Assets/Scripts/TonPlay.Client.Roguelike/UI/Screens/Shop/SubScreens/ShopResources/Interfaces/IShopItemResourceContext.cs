namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces
{
	public interface IShopItemResourceContext : IShopResourceContext
	{
		string ItemId { get; }
		string ItemDetailId { get; }
	}
}