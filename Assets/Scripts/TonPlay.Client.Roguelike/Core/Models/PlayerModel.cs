using System;
using TonPlay.Roguelike.Client.Core.Models.Data;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models
{
	public class PlayerModel : IPlayerModel
	{
		private readonly PlayerData _playerData = new PlayerData();
		private readonly SkillsModel _skillsModel = new SkillsModel();

		public IReadOnlyReactiveProperty<float> Health => _health;
		
		public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
		
		public IReadOnlyReactiveProperty<float> Experience => _experience;
		public IReadOnlyReactiveProperty<float> MaxExperience => _maxExperience;

		public ISkillsModel SkillsModel => _skillsModel;

		private ReactiveProperty<float> _health = new ReactiveProperty<float>();
		private ReactiveProperty<float> _maxHealth = new ReactiveProperty<float>();
		private ReactiveProperty<float> _experience = new ReactiveProperty<float>();
		private ReactiveProperty<float> _maxExperience = new ReactiveProperty<float>();

		public void Update(PlayerData data)
		{
			if (Math.Abs(_health.Value - data.Health) > 0.001f)
			{
				_health.SetValueAndForceNotify(data.Health);
			}
			
			if (Math.Abs(_maxHealth.Value - data.MaxHealth) > 0.001f)
			{
				_maxHealth.SetValueAndForceNotify(data.MaxHealth);
			}
			
			if (Math.Abs(_experience.Value - data.Experience) > 0.001f)
			{
				_experience.SetValueAndForceNotify(data.Experience);
			}
			
			if (Math.Abs(_maxExperience.Value - data.MaxExperience) > 0.001f)
			{
				_maxExperience.SetValueAndForceNotify(data.MaxExperience);
			}
			
			_skillsModel.Update(data.SkillsData);
		}
		
		public PlayerData ToData()
		{
			_playerData.Health = _health.Value;
			_playerData.MaxHealth = _maxHealth.Value;
			_playerData.Experience = _experience.Value;
			_playerData.MaxExperience = _maxExperience.Value;
			_playerData.SkillsData = _skillsModel.ToData();
			return _playerData;
		}
	}
}