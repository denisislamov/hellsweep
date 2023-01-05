using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Roguelike.Client.UI.UIService.Screen;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	public class GameScreen : Screen<GameScreenContext>
	{
		[SerializeField]
		private GameView _view;
		
		[Inject]
		private void Construct(GamePresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IGameScreenContext, GameScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}