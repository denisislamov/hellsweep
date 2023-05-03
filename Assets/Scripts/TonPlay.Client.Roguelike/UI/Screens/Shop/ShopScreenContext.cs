using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopScreenContext : ScreenContext, IShopScreenContext
	{
		public ShopNavTab InitialTab { get; }

		public ShopScreenContext(ShopNavTab initialTab)
		{
			InitialTab = initialTab;
		}
	}
}