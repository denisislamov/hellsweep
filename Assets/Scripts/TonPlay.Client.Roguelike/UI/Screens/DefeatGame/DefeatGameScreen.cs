using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Roguelike.Client.UI.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.DefeatGame
{
	public class DefeatGameScreen : Screen<DefeatGameScreenContext>
	{
		[SerializeField]
		private DefeatGameView _view;
		
		[Inject]
		private void Construct(DefeatGamePresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IDefeatGameScreenContext, DefeatGameScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}