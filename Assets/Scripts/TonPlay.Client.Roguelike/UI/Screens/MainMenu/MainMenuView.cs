using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
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

		public IButtonView PlayButton => _playButton;
		public IProfileBarView ProfileBarView => _profileBarView;
	}
}