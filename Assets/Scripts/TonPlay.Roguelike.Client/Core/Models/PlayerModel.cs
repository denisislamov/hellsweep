using TonPlay.Roguelike.Client.Core.Models.Data;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models
{
	public class PlayerModel : IPlayerModel
	{
		private readonly PlayerData _playerData = new PlayerData();
		
		public IReadOnlyReactiveProperty<int> Health => _health;
		
		public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;
		
		public IReadOnlyReactiveProperty<float> Experience => _experience;

		private ReactiveProperty<int> _health = new ReactiveProperty<int>();
		private ReactiveProperty<int> _maxHealth = new ReactiveProperty<int>();
		private ReactiveProperty<float> _experience = new ReactiveProperty<float>();

		public void Update(PlayerData data)
		{
			if (_health.Value != data.Health)
			{
				_health.SetValueAndForceNotify(data.Health);
			}
			
			if (_maxHealth.Value != data.MaxHealth)
			{
				_maxHealth.SetValueAndForceNotify(data.MaxHealth);
			}
			
			if (_experience.Value != data.Experience)
			{
				_experience.SetValueAndForceNotify(data.Experience);
			}
		}
		
		public PlayerData ToData()
		{
			_playerData.Health = _health.Value;
			_playerData.MaxHealth = _maxHealth.Value;
			return _playerData;
		}
	}
}