using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation
{
	public class NavigationMenuInstaller : Installer<NavigationMenuInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindFactory<INavigationMenuView, INavigationMenuContext, NavigationMenuPresenter, NavigationMenuPresenter.Factory>();
			Container.BindFactory<INavigationButtonView, INavigationButtonContext, NavigationButtonPresenter, NavigationButtonPresenter.Factory>();
		}
	}
}