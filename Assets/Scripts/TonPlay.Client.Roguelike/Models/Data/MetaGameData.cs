using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
	public class MetaGameData : IData
	{
		public ProfileData ProfileData { get; set; }

		public LocationsData LocationsData { get; set; }
		
		public UserLevelsInfoData UserLevelsInfoData { get; set; }
	}
}