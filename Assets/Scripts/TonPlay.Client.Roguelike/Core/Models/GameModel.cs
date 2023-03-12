using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class GameModel : IGameModel
	{
		private readonly PlayerModel _playerModel = new PlayerModel();
		private readonly ReactiveProperty<float> _gameTime = new ReactiveProperty<float>();
		private readonly ReactiveProperty<bool> _paused = new ReactiveProperty<bool>();

		private readonly GameData _cached = new GameData();
		private readonly ReactiveProperty<int> _deadEnemiesCount = new ReactiveProperty<int>();
		private readonly ReactiveProperty<int> _debugEnemyMovementToEachOtherCollisionCount = new ReactiveProperty<int>();

		public IPlayerModel PlayerModel => _playerModel;

		public IReadOnlyReactiveProperty<float> GameTime => _gameTime;

		public IReadOnlyReactiveProperty<bool> Paused => _paused;
		public IReadOnlyReactiveProperty<int> DeadEnemiesCount => _deadEnemiesCount;
		public IReadOnlyReactiveProperty<int> DebugEnemyMovementToEachOtherCollisionCount => _debugEnemyMovementToEachOtherCollisionCount;

		public void Update(GameData data)
		{
			_gameTime.SetValueAndForceNotify(data.GameTime);
			_paused.SetValueAndForceNotify(data.Paused);
			_deadEnemiesCount.SetValueAndForceNotify(data.DeadEnemies);
			_debugEnemyMovementToEachOtherCollisionCount.SetValueAndForceNotify(data.DebugEnemyMovementToEachOtherCollisionCount);

			_playerModel.Update(data.PlayerData);
		}

		public GameData ToData()
		{
			_cached.GameTime = _gameTime.Value;
			_cached.Paused = _paused.Value;
			_cached.DeadEnemies = _deadEnemiesCount.Value;

			_cached.PlayerData = _playerModel.ToData();
			return _cached;
		}
	}
}