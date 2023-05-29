using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLocationsLoadingService : IUserLoadingService
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ILocationConfigProvider _locationConfigProvider;
		private readonly IRestApiClient _restApiClient;
		
		public UserLocationsLoadingService(
			IMetaGameModelProvider metaGameModelProvider,
			ILocationConfigProvider locationConfigProvider,
			IRestApiClient restApiClient)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_locationConfigProvider = locationConfigProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
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
				var userLocation = userLocationsResponse.response.items.FirstOrDefault(_ => _.chapter == locationConfig.ChapterIdx);
				var userLocationSurviveMills = Convert.ToDouble(userLocation?.surviveMills ?? 0L);
				var userLocationFinished = userLocationSurviveMills >= TimeSpan.FromMinutes(14.99d).TotalMilliseconds;
				var maxKilled = userLocation?.maxKilled ?? 0;
				
				var locationData = new LocationData()
				{
					ChapterIdx = locationConfig.ChapterIdx,
					LongestSurvivedMillis = userLocationSurviveMills,
					Unlocked = !(userLocation is null),
					Won = userLocationFinished,
					MaxKilled = maxKilled
				};
				
				locationsData.Locations.Add(locationData.ChapterIdx, locationData);
			}
			
			locationsModel.Update(locationsData);
		}
		
		public class Factory : PlaceholderFactory<UserLocationsLoadingService>
		{
		}
	}
}