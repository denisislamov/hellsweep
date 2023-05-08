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
using UnityEngine;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLoadingService : IUserLoadingService
	{
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly ILocationConfigProvider _locationConfigProvider;
		private readonly IRestApiClient _restApiClient;

		public UserLoadingService(
			ILocationConfigProvider locationConfigProvider,
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IRestApiClient restApiClient)
		{
			_locationConfigProvider = locationConfigProvider;
			_restApiClient = restApiClient;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
		}

		public async UniTask Load()
		{
			await UpdateProfileModel();
			await UpdateLocationsModel();
			await UpdateInventoryModel();
			await UpdateGameSettingsModel();
			await UpdateShopModel();
		}
		
		private async UniTask UpdateShopModel()
		{
			var model = _metaGameModelProvider.Get().ShopModel;
			var data = model.ToData();
			
			var packsResponse = await _restApiClient.GetShopPacksAll();
			if (packsResponse.successful && packsResponse.response != null)
			{
				for (var i = 0; i < packsResponse.response.items.Count; i++)
				{
					var remotePack = packsResponse.response.items[i];
					
					data.Packs.Add(new ShopPackData()
					{
						Id = remotePack.id,
						Price = remotePack.priceTon,
						Rewards = new ShopPackRewardsData()
						{
							Blueprints = remotePack.blueprints,
							Coins = remotePack.coins,
							Energy = remotePack.energy,
							HeroSkins = remotePack.heroSkins,
							KeysCommon = remotePack.keysCommon,
							KeysUncommon = remotePack.keysUncommon,
							KeysRare = remotePack.keysRare,
							KeysLegendary = remotePack.keysLegendary,
						}
					});
				}
			}
			
			model.Update(data);
		}

		private async UniTask UpdateGameSettingsModel()
		{
			var gamePropertiesResponse = await _restApiClient.GetGameProperties();
			if (gamePropertiesResponse?.response.jsonData != null)
			{
				var gameSettingCached = gamePropertiesResponse.response.jsonData.gameSettings;

				var data = _metaGameModelProvider.Get().GameSettingsModel.ToData();

				data.MusicVolume = gameSettingCached.MusicVolume;
				data.SoundsVolume = gameSettingCached.SoundsVolume;
				data.ScreenGameStick = gameSettingCached.ScreenGameStick;
				data.VisualizeDamage = gameSettingCached.VisualizeDamage;

				_metaGameModelProvider.Get().GameSettingsModel.Update(data);
			}
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
			var itemsResponseTask = _restApiClient.GetUserItems();
			var slotsResponseTask = _restApiClient.GetUserSlots();
			var inventoryResponseTask = _restApiClient.GetUserInventory();

			var itemsResponse = await itemsResponseTask;
			var slotsResponse = await slotsResponseTask;
			var inventoryResponse = await inventoryResponseTask;

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel.InventoryModel;
			var data = model.ToData();
			
			for (var i = 0; i < itemsResponse.response.items.Count; i++)
			{
				var itemData = itemsResponse.response.items[i];
				var innerItemConfig = _inventoryItemsConfigProvider.GetInnerItemConfig(itemData.itemId);
				
				data.Items.Add(new InventoryItemData()
				{
					Id = itemData.id,
					ItemId = itemData.itemId,
					DetailId = itemData.itemDetailId,
				});
			}
			
			for (var i = 0; i < slotsResponse.response.items.Count; i++)
			{
				var slotData = slotsResponse.response.items[i];
				var slotName = (SlotName) Enum.Parse(typeof(SlotName), slotData.purpose, true);
				
				data.Slots.Add(slotName, new SlotData()
				{
					Id = slotData.id,
					SlotName = slotName,
					ItemId = slotData.userItemId
				});
			}

			for (var i = 0; i < 3; i++)
			{
				data.MergeSlots.Add(new SlotData()
				{
					Id = string.Empty,
					SlotName = SlotName.NECK,
					ItemId = string.Empty
				});
			}

			data.Blueprints = inventoryResponse.response.blueprints;

			model.Update(data);
		}
	}
}