using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLoadingService : IUserLoadingService
	{
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ILocationConfigProvider _locationConfigProvider;
		private readonly IRestApiClient _restApiClient;

		public UserLoadingService(
			ILocationConfigProvider locationConfigProvider,
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_locationConfigProvider = locationConfigProvider;
			_restApiClient = restApiClient;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
		}

		public async UniTask Load()
		{
			await UpdateProfileModel();
			await UpdateLocationsModel();
		}
		
		private async UniTask UpdateLocationsModel()
		{
			var userLocationsResponse = await _restApiClient.GetUserLocations();
			var metaGameModel = _metaGameModelProvider.Get();
			
			var locationsModel = metaGameModel.LocationsModel;
			var locationsData = locationsModel.ToData();
			
			locationsData.Locations.Clear();

			var locationConfigs = _locationConfigProvider.Configs;

			for (var i = 0; i < locationConfigs.Length; i++)
			{
				var locationConfig = locationConfigs[i];
				var userLocation = userLocationsResponse.items.FirstOrDefault(_ => _.chapter == locationConfig.ChapterIdx);
				var userLocationSurviveMills = Convert.ToDouble(userLocation?.surviveMills ?? 0L);
				var userLocationFinished = userLocationSurviveMills >= TimeSpan.FromMinutes(14.99d).TotalMilliseconds;
				var locationData = new LocationData()
				{
					ChapterIdx = locationConfig.ChapterIdx,
					LongestSurvivedMillis = userLocationSurviveMills,
					Unlocked = !(userLocation is null),
					Won = userLocationFinished
				};
				
				locationsData.Locations.Add(locationData.ChapterIdx, locationData);
			}
			
			locationsModel.Update(locationsData);
		}

		private async UniTask UpdateProfileModel()
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