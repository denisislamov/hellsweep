using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider
{
	public class LocationSliderContext : ScreenContext, ILocationSliderContext
	{
		public ILocationConfigStorageSelector LocationConfigStorageSelector { get; }

		public LocationSliderContext(ILocationConfigStorageSelector locationConfigStorageSelector)
		{
			LocationConfigStorageSelector = locationConfigStorageSelector;
		}
	}
}