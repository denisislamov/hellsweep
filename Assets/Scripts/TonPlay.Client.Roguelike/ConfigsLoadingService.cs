using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike
{
	public class ConfigsLoadingService : IConfigsLoadingService
	{
		private readonly IProfileConfigProviderUpdater _profileConfigProviderUpdater;
		private readonly ISkillConfigUpdater _skillConfigUpdater;
		private readonly IRestApiClient _restApiClient;

		public ConfigsLoadingService(
			IProfileConfigProviderUpdater profileConfigProviderUpdater,
			ISkillConfigUpdater skillConfigUpdater,
			IRestApiClient restApiClient)
		{
			_profileConfigProviderUpdater = profileConfigProviderUpdater;
			_skillConfigUpdater = skillConfigUpdater;
			_restApiClient = restApiClient;
		}

		public async UniTask Load()
		{
			await UpdateProfileConfigs();
			await UpdateSkillsConfigs();
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
	}
}