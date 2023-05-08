using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike
{
	public class ConfigsLoadingService : IConfigsLoadingService
	{
		private readonly IProfileConfigProviderUpdater _profileConfigProviderUpdater;
		private readonly ILocationConfigUpdater _locationConfigUpdater;
		private readonly ISkillConfigUpdater _skillConfigUpdater;
		private readonly IInventoryItemsConfigUpdater _itemsConfigUpdater;
		private readonly IRestApiClient _restApiClient;

		public ConfigsLoadingService(
			IProfileConfigProviderUpdater profileConfigProviderUpdater,
			ILocationConfigUpdater locationConfigUpdater,
			ISkillConfigUpdater skillConfigUpdater,
			IInventoryItemsConfigUpdater itemsConfigUpdater,
			IRestApiClient restApiClient)
		{
			_profileConfigProviderUpdater = profileConfigProviderUpdater;
			_locationConfigUpdater = locationConfigUpdater;
			_skillConfigUpdater = skillConfigUpdater;
			_itemsConfigUpdater = itemsConfigUpdater;
			_restApiClient = restApiClient;
		}

		public async UniTask Load()
		{
			await UniTask.WhenAll(
				UpdateProfileConfigs(),
				UpdateSkillsConfigs(),
				UpdateLocationsConfigs(),
				UpdateItemsConfigs(),
				UpdateItemsCostsConfigs()
			);
		}

		private async UniTask UpdateLocationsConfigs()
		{
			var response = await _restApiClient.GetLocationAll();

			for (int i = 0; i < response.response.items.Count; i++)
			{
				var locationConfig = response.response.items[i];
				_locationConfigUpdater.UpdateByIndex(locationConfig.chapter, locationConfig);
			}
		}

		private async UniTask UpdateProfileConfigs()
		{
			var response = await _restApiClient.GetInfoLevelAll();

			for (var i = 0; i < response.response.items.Count; i++)
			{
				var itemConfig = response.response.items[i];
				_profileConfigProviderUpdater.UpdateConfigExperienceToLevelUp(itemConfig.level, itemConfig.xp);
			}
		}

		private async UniTask UpdateSkillsConfigs()
		{
			var skillAllResponse = await _restApiClient.GetSkillAll();
			var boostAllResponse = await _restApiClient.GetBoostAll();

			for (var i = 0; i < skillAllResponse.response.items.Count; i++)
			{
				var remoteConfig = skillAllResponse.response.items[i];
				var name = RemoteSkillConverter.ConvertUdidToSkillName(remoteConfig.id);

				_skillConfigUpdater.UpdateConfig(name, remoteConfig);
			}

			for (var i = 0; i < boostAllResponse.response.items.Count; i++)
			{
				var remoteConfig = boostAllResponse.response.items[i];
				var name = RemoteSkillConverter.ConvertUdidToSkillName(remoteConfig.id);

				_skillConfigUpdater.UpdateConfig(name, remoteConfig);
			}
		}

		private async UniTask UpdateItemsConfigs()
		{
			var response = await _restApiClient.GetAllItems();

			for (var i = 0; i < response.response.items.Count; i++)
			{
				var remoteConfig = response.response.items[i];

				_itemsConfigUpdater.Update(remoteConfig.id, remoteConfig);
			}

			UpdateItemRarenessConfigs(response.response);
		}
		
		private async UniTask UpdateItemsCostsConfigs()
		{
			var response = await _restApiClient.GetItemLevelRatesAll();

			for (ushort i = 0; i < response.response.items.Count; i++)
			{
				var remoteConfig = response.response.items[i];

				_itemsConfigUpdater.UpdateItemUpgradePrices((ushort)(i + 1), remoteConfig);
			}
		}

		private void UpdateItemRarenessConfigs(ItemsGetResponse response)
		{
			_itemsConfigUpdater.UpdateItemRarenessConfigs(response.items);
		}
	}
}