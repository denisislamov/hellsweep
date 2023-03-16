using TonPlay.Client.Roguelike.Core.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Models
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