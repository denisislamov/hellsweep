using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface ILocationModel : IModel<LocationData>
	{
		string Id { get; }

		IReadOnlyReactiveProperty<double> LongestSurvivedMillis { get; }
		
		IReadOnlyReactiveProperty<bool> Won { get; }
		
		IReadOnlyReactiveProperty<bool> Unlocked { get; }
	}
}