using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopScreenContext : IScreenContext
	{
		ShopNavTabName InitialTabName { get; }
	}
}