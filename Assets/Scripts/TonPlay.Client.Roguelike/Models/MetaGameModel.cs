using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class MetaGameModel : IMetaGameModel
	{
		private readonly MetaGameData _cached = new MetaGameData();

		public IProfileModel ProfileModel { get; } = new ProfileModel();
		public ILocationsModel LocationsModel { get; } = new LocationsModel();

		private IUserLevelsInfoModel UserLevelsInfoModel { get; } = new UserLevelsInfoModel();

		public void Update(MetaGameData data)
		{
			ProfileModel.Update(data.ProfileData);
			LocationsModel.Update(data.LocationsData);
			UserLevelsInfoModel.Update(data.UserLevelsInfoData);
		}

		public MetaGameData ToData()
		{
			_cached.ProfileData = ProfileModel.ToData();
			_cached.LocationsData = LocationsModel.ToData();
			_cached.UserLevelsInfoData = UserLevelsInfoModel.ToData();
			
			return _cached;
		}
	}
}