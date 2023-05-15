using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopScreenContext : ScreenContext, IShopScreenContext
	{
		public ShopNavTabName InitialTabName { get; }

		public ShopScreenContext(ShopNavTabName initialTabName)
		{
			InitialTabName = initialTabName;
		}
	}
}