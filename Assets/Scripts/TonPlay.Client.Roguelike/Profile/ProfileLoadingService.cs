using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike.Profile
{
	public class ProfileLoadingService : IProfileLoadingService
	{
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;

		public ProfileLoadingService(
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_restApiClient = restApiClient;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
		}

		public async UniTask Load()
		{
			var userSummaryResponse = await _restApiClient.GetUserSummary();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();

			data.Level = userSummaryResponse.profile.level;
			data.Experience = userSummaryResponse.profile.xp;
			data.BalanceData.Gold = userSummaryResponse.profile.coin;
			data.BalanceData.Energy = userSummaryResponse.profile.energy; 
			data.BalanceData.MaxEnergy = userSummaryResponse.profile.energyMax;
			
			var config = _profileConfigProvider.Get(data.Level);

			data.MaxExperience = config?.ExperienceToLevelUp ?? int.MaxValue;

			model.Update(data);
		}
	}
}