using TonPlay.Roguelike.Client.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.MainMenu
{
	internal class MainMenuPresenter : Presenter<IMainMenuView, IMainMenuScreenContext>
	{
		public MainMenuPresenter(
			IMainMenuView view, 
			IMainMenuScreenContext context) 
			: base(view, context)
		{
		}
		
		internal class Factory : PlaceholderFactory<IMainMenuView, IMainMenuScreenContext, MainMenuPresenter>
		{
		}
	}
}