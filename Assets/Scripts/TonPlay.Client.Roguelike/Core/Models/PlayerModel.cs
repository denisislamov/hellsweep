using System;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class PlayerModel : IPlayerModel
	{
		private readonly PlayerData _playerData = new PlayerData();
		private readonly SkillsModel _skillsModel = new SkillsModel();
		private readonly MatchProfileGainModel _matchProfileGainModel = new MatchProfileGainModel();

		private readonly ReactiveProperty<float> _health = new ReactiveProperty<float>();
		private readonly ReactiveProperty<float> _maxHealth = new ReactiveProperty<float>();
		private readonly ReactiveProperty<float> _experience = new ReactiveProperty<float>();
		private readonly ReactiveProperty<float> _maxExperience = new ReactiveProperty<float>();
		private readonly ReactiveProperty<Vector2> _position = new ReactiveProperty<Vector2>();

		public IReadOnlyReactiveProperty<float> Health => _health;
		public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
		public IReadOnlyReactiveProperty<float> Experience => _experience;
		public IReadOnlyReactiveProperty<float> MaxExperience => _maxExperience;
		public IReadOnlyReactiveProperty<Vector2> Position => _position;
		public ISkillsModel SkillsModel => _skillsModel;
		public IMatchProfileGainModel MatchProfileGainModel => _matchProfileGainModel;

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
			
			if ((data.Position - _position.Value).sqrMagnitude > 0.001f)
			{
				_position.SetValueAndForceNotify(data.Position);
			}

			_skillsModel.Update(data.SkillsData);
			_matchProfileGainModel.Update(data.MatchProfileGainModel);
		}

		public PlayerData ToData()
		{
			_playerData.Health = _health.Value;
			_playerData.MaxHealth = _maxHealth.Value;
			_playerData.Experience = _experience.Value;
			_playerData.MaxExperience = _maxExperience.Value;
			_playerData.Position = _position.Value;
			_playerData.SkillsData = _skillsModel.ToData();
			_playerData.MatchProfileGainModel = _matchProfileGainModel.ToData();
			return _playerData;
		}
	}
}