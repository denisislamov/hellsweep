using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class LocationsData : IData
	{
		public Dictionary<int, LocationData> Locations { get; set; }
	}
	
	
}