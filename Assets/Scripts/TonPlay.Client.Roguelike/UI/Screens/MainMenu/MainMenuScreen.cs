using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class MainMenuScreen : Screen<MainMenuScreenContext>
	{
		[SerializeField]
		private MainMenuView _view;

		[Inject]
		private void Construct(MainMenuPresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IMainMenuScreenContext, MainMenuScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}