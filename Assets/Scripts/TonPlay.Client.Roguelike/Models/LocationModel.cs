using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	public class LocationModel : ILocationModel
	{
		private readonly LocationData _cached = new LocationData();

		private int _chapterIdx;
		private readonly ReactiveProperty<double> _longestSurvivedMillis = new ReactiveProperty<double>();
		private readonly ReactiveProperty<bool> _won = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<bool> _unlocked = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<long> _maxKilled = new ReactiveProperty<long>();
		public int ChapterIdx => _chapterIdx;
		public IReadOnlyReactiveProperty<double> LongestSurvivedMillis => _longestSurvivedMillis;
		public IReadOnlyReactiveProperty<bool> Won => _won;
		public IReadOnlyReactiveProperty<bool> Unlocked => _unlocked;
		public IReadOnlyReactiveProperty<long> MaxKilled => _maxKilled;

		public void Update(LocationData data)
		{
			_chapterIdx = data.ChapterIdx;

			if (data.Won != _won.Value)
			{
				_won.SetValueAndForceNotify(data.Won);
			}
			
			if (data.Unlocked != _unlocked.Value)
			{
				_unlocked.SetValueAndForceNotify(data.Unlocked);
			}

			_longestSurvivedMillis.SetValueAndForceNotify(data.LongestSurvivedMillis);
			_maxKilled.SetValueAndForceNotify(data.MaxKilled);
		}

		public LocationData ToData()
		{
			_cached.ChapterIdx = _chapterIdx;
			_cached.Won = _won.Value;
			_cached.LongestSurvivedMillis = _longestSurvivedMillis.Value;
			_cached.MaxKilled = _maxKilled.Value;
			
			return _cached;
		}
	}
}