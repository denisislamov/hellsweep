using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class LocationsModel : ILocationsModel
	{
		private readonly LocationsData _cached = new LocationsData();

		private Dictionary<string, ILocationModel> _locations = new Dictionary<string, ILocationModel>();

		public IReadOnlyDictionary<string, ILocationModel> Locations => _locations;

		public void Update(LocationsData data)
		{
			foreach (var kvp in data.Locations)
			{
				if (!_locations.ContainsKey(kvp.Key))
				{
					_locations.Add(kvp.Key, new LocationModel());
				}

				_locations[kvp.Key].Update(kvp.Value);
			}

			foreach (var kvp in _locations)
			{
				if (!data.Locations.ContainsKey(kvp.Key))
				{
					_locations.Remove(kvp.Key);
				}
			}
		}

		public LocationsData ToData()
		{
			_cached.Locations = _locations.ToDictionary(_ => _.Key, _ => _.Value.ToData());

			return _cached;
		}
	}
}