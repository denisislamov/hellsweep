using TonPlay.Roguelike.Client.Core.Models.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Models
{
	internal class GameModelProvider : IGameModelProvider, IGameModelSetter
	{
		private IGameModel _gameModel;

		public IGameModel Get()
		{
			return _gameModel;
		}
		
		public void Set(IGameModel gameModel)
		{
			_gameModel = gameModel;
		}
	}
}