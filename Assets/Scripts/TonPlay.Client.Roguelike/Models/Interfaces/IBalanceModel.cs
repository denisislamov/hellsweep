using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IBalanceModel : IModel<BalanceData>
	{
		IReadOnlyReactiveProperty<int> Gold { get; }

		IReadOnlyReactiveProperty<int> Energy { get; }

		IReadOnlyReactiveProperty<int> MaxEnergy { get; }
	}
}