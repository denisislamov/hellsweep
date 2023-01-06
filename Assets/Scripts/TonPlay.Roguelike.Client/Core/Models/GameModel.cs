using TonPlay.Roguelike.Client.Core.Models.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Models
{
	public class GameModel : IGameModel
	{
		public IPlayerModel PlayerModel => _playerModel;
		
		private PlayerModel _playerModel = new PlayerModel();
	}
}