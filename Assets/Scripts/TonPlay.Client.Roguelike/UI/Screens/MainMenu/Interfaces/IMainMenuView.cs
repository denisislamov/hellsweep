using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
	public interface IMainMenuView : IView
	{
		Button PlayButton { get; }
	}
}