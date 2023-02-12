using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class MainMenuView : View, IMainMenuView
	{
		[SerializeField]
		private ButtonView _playButton;
		
		[SerializeField]
		private ProfileBarView _profileBarView;
		
		[SerializeField]
		private LocationSliderView _locationSliderView;

		public IButtonView PlayButton => _playButton;
		public IProfileBarView ProfileBarView => _profileBarView;
		public ILocationSliderView LocationSliderView => _locationSliderView;
	}
}