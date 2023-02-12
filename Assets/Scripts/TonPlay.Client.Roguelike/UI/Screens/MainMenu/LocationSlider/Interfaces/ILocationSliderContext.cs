using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces
{
	public interface ILocationSliderContext : IScreenContext
	{
		ILocationConfigStorageSelector LocationConfigStorageSelector { get; }
	}
}