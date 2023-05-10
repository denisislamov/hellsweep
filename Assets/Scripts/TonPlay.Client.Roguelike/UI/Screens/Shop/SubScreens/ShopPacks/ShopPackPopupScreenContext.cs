using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackPopupScreenContext : ScreenContext, IShopPackPopupScreenContext
	{
		public IShopPackModel Model { get; }
		
		public ShopPackPopupScreenContext(IShopPackModel model)
		{
			Model = model;
		}
	}
}