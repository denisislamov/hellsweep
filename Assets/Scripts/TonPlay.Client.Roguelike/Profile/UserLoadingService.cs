using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
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
			await UpdateInventoryModel();
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
				var userLocation = userLocationsResponse.response.items.FirstOrDefault(_ => _.chapter == locationConfig.ChapterIdx);
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

			data.Level = userSummaryResponse.response.profile.level;
			data.Experience = userSummaryResponse.response.profile.xp;
			data.BalanceData.Gold = userSummaryResponse.response.profile.coin;
			data.BalanceData.Energy = userSummaryResponse.response.profile.energy;
			data.BalanceData.MaxEnergy = userSummaryResponse.response.profile.energyMax;

			var config = _profileConfigProvider.Get(data.Level);

			data.MaxExperience = config?.ExperienceToLevelUp ?? int.MaxValue;

			model.Update(data);
		}
		
		private async UniTask UpdateInventoryModel()
		{
			var itemsResponse = await _restApiClient.GetUserItems();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel.InventoryModel;
			var data = model.ToData();
			
			for (var i = 0; i < itemsResponse.response.items.Count; i++)
			{
				var itemData = itemsResponse.response.items[i];
				data.Items.Add(new InventoryItemData()
				{
					Id = itemData.id,
					DetailId = itemData.item.id,
					Level = itemData.level
				});
			}
			
			var slotsResponse = await _restApiClient.GetUserSlots();
			
			for (var i = 0; i < slotsResponse.response.items.Count; i++)
			{
				var slotData = slotsResponse.response.items[i];
				var item = new InventoryItemData(){ Id = slotData.itemDetail?.id, DetailId = slotData.itemDetail?.item?.id} ;
				
				var slotName = (SlotName) Enum.Parse(typeof(SlotName), slotData.purpose, true);
				
				data.Slots.Add(slotName, new SlotData()
				{
					Id = slotData.id,
					SlotName = slotName,
					Item = item
				});
			}

			model.Update(data);
		}
	}
}