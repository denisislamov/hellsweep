using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class MetaGameModel : IMetaGameModel
	{
		private readonly MetaGameData _cached = new MetaGameData();

		public IProfileModel ProfileModel { get; } = new ProfileModel();
		public ILocationsModel LocationsModel { get; } = new LocationsModel();

		public void Update(MetaGameData data)
		{
			ProfileModel.Update(data.ProfileData);
			LocationsModel.Update(data.LocationsData);
		}
		
		public MetaGameData ToData()
		{
			_cached.ProfileData = ProfileModel.ToData();
			_cached.LocationsData = LocationsModel.ToData();
			return _cached;
		}
	}
}