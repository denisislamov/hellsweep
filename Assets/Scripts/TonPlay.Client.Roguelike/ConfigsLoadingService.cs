using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike
{
	public class ConfigsLoadingService : IConfigsLoadingService
	{
		private readonly IProfileConfigProviderUpdater _profileConfigProviderUpdater;
		private readonly IRestApiClient _restApiClient;

		public ConfigsLoadingService(
			IProfileConfigProviderUpdater profileConfigProviderUpdater,
			IRestApiClient restApiClient)
		{
			_profileConfigProviderUpdater = profileConfigProviderUpdater;
			_restApiClient = restApiClient;
		}

		public async UniTask Load()
		{
			await UpdateProfileConfigs();
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
	}
}