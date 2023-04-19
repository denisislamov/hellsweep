using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	public class BalanceModel : IBalanceModel
	{
		private readonly BalanceData _cached = new BalanceData();

		private readonly ReactiveProperty<long> _gold = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprints = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _energy = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _maxEnergy = new ReactiveProperty<long>();

		public IReadOnlyReactiveProperty<long> Gold => _gold;
		public IReadOnlyReactiveProperty<long> Blueprints => _blueprints;
		public IReadOnlyReactiveProperty<long> Energy => _energy;
		public IReadOnlyReactiveProperty<long> MaxEnergy => _maxEnergy;

		public void Update(BalanceData data)
		{
			if (data.Gold != _gold.Value)
			{
				_gold.SetValueAndForceNotify(data.Gold);
			}

			if (data.Energy != _energy.Value)
			{
				_energy.SetValueAndForceNotify(data.Energy);
			}

			if (data.MaxEnergy != _maxEnergy.Value)
			{
				_maxEnergy.SetValueAndForceNotify(data.MaxEnergy);
			}
		}

		public BalanceData ToData()
		{
			_cached.Gold = Gold.Value;
			_cached.Energy = Energy.Value;
			_cached.MaxEnergy = MaxEnergy.Value;
			return _cached;
		}
	}
}