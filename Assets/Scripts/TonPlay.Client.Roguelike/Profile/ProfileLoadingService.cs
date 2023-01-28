using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike.Profile
{
	public class ProfileLoadingService : IProfileLoadingService
	{
		private readonly IProfileConfigProvider _configProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		public ProfileLoadingService(
			IProfileConfigProvider configProvider,
			IMetaGameModelProvider metaGameModelProvider)
		{
			_configProvider = configProvider;
			_metaGameModelProvider = metaGameModelProvider;
		}
		
		public async UniTask Load()
		{
			var config = _configProvider.Get(1);
			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();
			
			data.Level = config.Level;
			data.MaxExperience = config.ExperienceToLevelUp;
			data.BalanceData.Energy = config.MaxEnergy;
			data.BalanceData.MaxEnergy = config.MaxEnergy;
			
			model.Update(data);
		}
	}
}