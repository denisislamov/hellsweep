using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserGameSettingsLoadingService : IUserLoadingService
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		private UserLoadingService _userLoadingService;
		public UserGameSettingsLoadingService(
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
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
		
		public class Factory : PlaceholderFactory<UserGameSettingsLoadingService>
		{
		}
	}
}