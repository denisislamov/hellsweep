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
		private ReactiveProperty<double> _longestSurvivedMillis = new ReactiveProperty<double>();

		public string Id => _id;
		public IReadOnlyReactiveProperty<double> LongestSurvivedMillis => _longestSurvivedMillis;

		public void Update(LocationData data)
		{
			if (string.IsNullOrEmpty(_id))
			{
				_id = data.Id;
			}

			_longestSurvivedMillis.SetValueAndForceNotify(data.LongestSurvivedMillis);
		}

		public LocationData ToData()
		{
			_cached.Id = _id;
			_cached.LongestSurvivedMillis = _longestSurvivedMillis.Value;

			return _cached;
		}
	}
}