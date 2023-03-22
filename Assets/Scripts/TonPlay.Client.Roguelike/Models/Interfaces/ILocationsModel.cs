using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface ILocationsModel : IModel<LocationsData>
	{
		IReadOnlyDictionary<int, ILocationModel> Locations { get; }
	}
}