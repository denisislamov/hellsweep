using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class LocationData : IData
	{
		public string Id { get; set; }

		public double LongestSurvivedMillis { get; set; }
		
		public bool Won { get; set; }
		
		public bool Unlocked { get; set; }
	}
}