using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserProfileLoadingService : IUserLoadingService
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IRestApiClient _restApiClient;
		
		public UserProfileLoadingService(
			IMetaGameModelProvider metaGameModelProvider,
			IProfileConfigProvider profileConfigProvider,
			IRestApiClient restApiClient)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_profileConfigProvider = profileConfigProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
		{
			var userSummaryResponse = await _restApiClient.GetUserSummary();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();

			data.Level = userSummaryResponse.response.profile.level;
			data.Experience = userSummaryResponse.response.profile.xp;
			data.BalanceData.Gold = userSummaryResponse.response.profile.coin;
			data.BalanceData.Energy = userSummaryResponse.response.profile.energy;
			data.BalanceData.MaxEnergy = userSummaryResponse.response.profile.energyMax;

			var config = _profileConfigProvider.Get(data.Level);

			data.MaxExperience = config?.ExperienceToLevelUp ?? int.MaxValue;

			model.Update(data);
		}
		
		public class Factory : PlaceholderFactory<UserProfileLoadingService>
		{
		}
	}
}