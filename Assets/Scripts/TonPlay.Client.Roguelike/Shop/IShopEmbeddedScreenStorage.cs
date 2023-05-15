using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopEmbeddedScreenStorage
	{
		IScreen Current { get; }
		
		void Set(IScreen screen);
	}
}