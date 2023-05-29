using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class LocationData : IData
	{
		public int ChapterIdx { get; set; } 

		public double LongestSurvivedMillis { get; set; }
		
		public bool Won { get; set; }
		
		public bool Unlocked { get; set; }

		public long MaxKilled { get; set; }
	}
}