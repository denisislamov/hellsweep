using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	public class LocationModel : ILocationModel
	{
		private readonly LocationData _cached = new LocationData();

		private string _id;
		private readonly ReactiveProperty<double> _longestSurvivedMillis = new ReactiveProperty<double>();
		private readonly ReactiveProperty<bool> _won = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<bool> _unlocked = new ReactiveProperty<bool>();

		public string Id => _id;
		public IReadOnlyReactiveProperty<double> LongestSurvivedMillis => _longestSurvivedMillis;
		public IReadOnlyReactiveProperty<bool> Won => _won;
		public IReadOnlyReactiveProperty<bool> Unlocked => _unlocked;

		public void Update(LocationData data)
		{
			if (string.IsNullOrEmpty(_id))
			{
				_id = data.Id;
			}

			if (data.Won != _won.Value)
			{
				_won.SetValueAndForceNotify(data.Won);
			}
			
			if (data.Unlocked != _unlocked.Value)
			{
				_unlocked.SetValueAndForceNotify(data.Unlocked);
			}

			_longestSurvivedMillis.SetValueAndForceNotify(data.LongestSurvivedMillis);
		}

		public LocationData ToData()
		{
			_cached.Id = _id;
			_cached.Won = _won.Value;
			_cached.LongestSurvivedMillis = _longestSurvivedMillis.Value;

			return _cached;
		}
	}
}