using TonPlay.Roguelike.Client.Core.Models.Data;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models
{
	public class GameModel : IGameModel
	{
		private readonly PlayerModel _playerModel = new PlayerModel();

		private readonly ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();

		private readonly GameData _cached = new GameData();
		
		public IPlayerModel PlayerModel => _playerModel;
		
		public IReadOnlyReactiveProperty<float> GameTime => _gameTime;

		public void Update(GameData data)
		{
			_gameTime.SetValueAndForceNotify(data.GameTime);
			
			_playerModel.Update(data.PlayerData);
		}
		
		public GameData ToData()
		{
			_cached.GameTime = _gameTime.Value;
			_cached.PlayerData = _playerModel.ToData();
			return _cached;
		}
	}
}