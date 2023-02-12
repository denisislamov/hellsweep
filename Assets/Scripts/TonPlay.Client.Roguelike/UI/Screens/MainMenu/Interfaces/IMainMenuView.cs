using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
	public interface IMainMenuView : IView
	{
		IButtonView PlayButton { get; }
		
		IProfileBarView ProfileBarView { get; }
		
		ILocationSliderView LocationSliderView { get; }
	}
}