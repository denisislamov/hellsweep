using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class MainMenuView : View, IMainMenuView
	{
		[SerializeField]
		private Button _playButton;

		public Button PlayButton => _playButton;
	}
}