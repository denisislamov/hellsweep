using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
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
			await UpdateProfileConfigs();
			await UpdateSkillsConfigs();
			await UpdateLocationsConfigs();
			await UpdateItemsConfigs();
		}
		
		private async UniTask UpdateLocationsConfigs()
		{
			var response = await _restApiClient.GetLocationAll();

			for (int i = 0; i < response.items.Count; i++)
			{
				var locationConfig = response.items[i];
				_locationConfigUpdater.UpdateByIndex(locationConfig.chapter, locationConfig);
			}
		}

		private async UniTask UpdateProfileConfigs()
		{
			var response = await _restApiClient.GetInfoLevelAll();

			for (var i = 0; i < response.items.Count; i++)
			{
				var itemConfig = response.items[i];
				_profileConfigProviderUpdater.UpdateConfigExperienceToLevelUp(itemConfig.level, itemConfig.xp);
			}
		}
		
		private async UniTask UpdateSkillsConfigs()
		{
			var skillAllResponse = await _restApiClient.GetSkillAll();
			var boostAllResponse = await _restApiClient.GetBoostAll();

			for (var i = 0; i < skillAllResponse.items.Count; i++)
			{
				var remoteConfig = skillAllResponse.items[i];
				var name = RemoteSkillConverter.ConvertUdidToSkillName(remoteConfig.id);

				_skillConfigUpdater.UpdateConfig(name, remoteConfig);
			}
			
			for (var i = 0; i < boostAllResponse.items.Count; i++)
			{
				var remoteConfig = boostAllResponse.items[i];
				var name = RemoteSkillConverter.ConvertUdidToSkillName(remoteConfig.id);

				_skillConfigUpdater.UpdateConfig(name, remoteConfig);
			}
		}
		
		private async UniTask UpdateItemsConfigs()
		{
			var response = await _restApiClient.GetAllItems();

			for (var i = 0; i < response.items.Count; i++)
			{
				var remoteConfig = response.items[i];
				
				_itemsConfigUpdater.Update(remoteConfig.id, remoteConfig);
			}
		}
	}
}