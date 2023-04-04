using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	public class ProfileModel : IProfileModel
	{
		private readonly ProfileData _cached = new ProfileData();

		private readonly ReactiveProperty<float> _experience = new ReactiveProperty<float>();
		private readonly ReactiveProperty<float> _maxExperience = new ReactiveProperty<float>();
		private readonly ReactiveProperty<int> _level = new ReactiveProperty<int>();

		public IReadOnlyReactiveProperty<int> Level => _level;
		public IReadOnlyReactiveProperty<float> Experience => _experience;
		public IReadOnlyReactiveProperty<float> MaxExperience => _maxExperience;

		
		public IBalanceModel BalanceModel { get; } = new BalanceModel();
		public IInventoryModel InventoryModel { get; } = new InventoryModel();

		public void Update(ProfileData data)
		{
			if (Math.Abs(data.Experience - _experience.Value) > 0.000001f)
			{
				_experience.SetValueAndForceNotify(data.Experience);
			}

			if (Math.Abs(data.MaxExperience - _maxExperience.Value) > 0.000001f)
			{
				_maxExperience.SetValueAndForceNotify(data.MaxExperience);
			}

			if (data.Level != _level.Value)
			{
				_level.SetValueAndForceNotify(data.Level);
			}

			BalanceModel.Update(data.BalanceData);
			InventoryModel.Update(data.InventoryData);
		}

		public ProfileData ToData()
		{
			_cached.Experience = _experience.Value;
			_cached.MaxExperience = _maxExperience.Value;

			_cached.BalanceData = BalanceModel.ToData();
			_cached.InventoryData = InventoryModel.ToData();
			return _cached;
		}
	}
}