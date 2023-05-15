using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.Shop
{
	public class ShopEmbeddedScreenStorage : IShopEmbeddedScreenStorage
	{
		public IScreen Current { get; private set; }

		public void Set(IScreen screen)
		{
			Current = screen;
		}
	}
}