using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IBalanceModel : IModel<BalanceData>
	{
		IReadOnlyReactiveProperty<long> Gold { get; }
		
		IReadOnlyReactiveProperty<long> Energy { get; }

		IReadOnlyReactiveProperty<long> MaxEnergy { get; }
	}
}