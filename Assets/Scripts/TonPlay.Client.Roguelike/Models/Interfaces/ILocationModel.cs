using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface ILocationModel : IModel<LocationData>
	{
		public string Id { get; }
		
		public IReadOnlyReactiveProperty<double> LongestSurvivedMillis { get; }
	}
}