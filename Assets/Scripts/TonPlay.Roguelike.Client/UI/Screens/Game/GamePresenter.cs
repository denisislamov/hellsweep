using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.Game
{
	internal class GamePresenter : Presenter<IGameView, IGameScreenContext>
	{
		public GamePresenter(
			IGameView view, 
			IGameScreenContext context) 
			: base(view, context)
		{
		}
		
		internal class Factory : PlaceholderFactory<IGameView, IGameScreenContext, GamePresenter>
		{
		}
	}
}