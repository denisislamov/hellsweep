using System;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class BossModel : IBossModel
	{
		private readonly BossData _cachedData = new BossData();
		
		private readonly ReactiveProperty<bool> _exists = new ReactiveProperty<bool>();
		private readonly ReactiveProperty<float> _health = new ReactiveProperty<float>();
		private readonly ReactiveProperty<float> _maxHealth = new ReactiveProperty<float>();

		public IReadOnlyReactiveProperty<bool> Exists => _exists;
		public IReadOnlyReactiveProperty<float> Health => _health;
		public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;

		public void Update(BossData data)
		{
			if (data.Exists != _exists.Value)
			{
				_exists.SetValueAndForceNotify(data.Exists);
			}

			if (Math.Abs(_health.Value - data.Health) > 0.001f)
			{
				_health.SetValueAndForceNotify(data.Health);
			}

			if (Math.Abs(_maxHealth.Value - data.MaxHealth) > 0.001f)
			{
				_maxHealth.SetValueAndForceNotify(data.MaxHealth);
			}
		}

		public BossData ToData()
		{
			_cachedData.Exists = _exists.Value;
			_cachedData.Health = _health.Value;
			_cachedData.MaxHealth = _maxHealth.Value;
			return _cachedData;
		}
	}
}