using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class LocationsData : IData
	{
		public Dictionary<string, LocationData> Locations { get; set; }
	}
}