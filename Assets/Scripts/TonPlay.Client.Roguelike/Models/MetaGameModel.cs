using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class MetaGameModel : IMetaGameModel
	{
		private readonly MetaGameData _cached = new MetaGameData();

		public IProfileModel ProfileModel { get; } = new ProfileModel();

		public void Update(MetaGameData data)
		{
			ProfileModel.Update(data.ProfileData);
		}
		
		public MetaGameData ToData()
		{
			_cached.ProfileData = ProfileModel.ToData();
			return _cached;
		}
	}
}