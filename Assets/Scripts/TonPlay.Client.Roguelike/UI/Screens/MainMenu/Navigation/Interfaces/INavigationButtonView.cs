using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces
{
	public interface INavigationButtonView : IView
	{
		IButtonView ButtonView { get; }
		
		void SetActiveState(bool state);
	}
}