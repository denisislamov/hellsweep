using System;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class MatchProfileGainModel : IMatchProfileGainModel
	{
		private readonly MatchProfileGainData _cached = new MatchProfileGainData();
		
		private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>();
		private readonly ReactiveProperty<float> _profileExperience = new ReactiveProperty<float>();
		
		public IReadOnlyReactiveProperty<int> Gold => _gold;
		public IReadOnlyReactiveProperty<float> ProfileExperience => _profileExperience;
		
		public void Update(MatchProfileGainData data)
		{
			if (Math.Abs(_profileExperience.Value - data.ProfileExperience) > 0.001f)
			{
				_profileExperience.SetValueAndForceNotify(data.ProfileExperience);
			}

			if (_gold.Value != data.Gold)
			{
				_gold.SetValueAndForceNotify(data.Gold);
			}
		}
		
		public MatchProfileGainData ToData()
		{
			_cached.Gold = _gold.Value;
			_cached.ProfileExperience = _profileExperience.Value;
			return _cached;
		}
	}
}