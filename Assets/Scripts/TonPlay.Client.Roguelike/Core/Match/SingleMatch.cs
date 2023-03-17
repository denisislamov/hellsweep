using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Match
{
	public class SingleMatch : IMatch
	{
		private readonly ISceneService _sceneService;
		private readonly IUIService _uiService;
		private readonly IGameModelProvider _gameModelProvider;
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ILocationConfig _locationConfig;
		private readonly IRestApiClient _restApiClient;

		public SingleMatch(
			ILocationConfig locationConfig,
			ISceneService sceneService,
			IUIService uiService,
			IGameModelProvider gameModelProvider,
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
		{
			_sceneService = sceneService;
			_uiService = uiService;
			_gameModelProvider = gameModelProvider;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_locationConfig = locationConfig;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Launch()
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var data = balanceModel.ToData();

			if (data.Energy < RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE)
			{
				return;
			}

			data.Energy -= RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE;

			balanceModel.Update(data);
			
			await _restApiClient.PostGameSession(true); 
			
			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				await _restApiClient.PostGameSessionClose(new GameSessionPostBody()
				{
					surviveMills = 0
				});
			}

			await _restApiClient.PostGameSession(true); // TODO - add PVE variable
			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(_locationConfig.SceneName);
		}
		
		public async UniTask Finish()
		{
			var metaGameModel = _metaGameModelProvider.Get();

			var locationsModel = metaGameModel.LocationsModel;
			var locationsData = locationsModel.ToData();

			if (!locationsData.Locations.ContainsKey(_locationConfig.Id))
			{
				locationsData.Locations.Add(
					_locationConfig.Id,
					new LocationData() {Id = _locationConfig.Id});
			}

			var locationData = locationsData.Locations[_locationConfig.Id];

			var profileModel = metaGameModel.ProfileModel;
			var gameModel = _gameModelProvider.Get();

			var profileData = profileModel.ToData();

			profileData.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
			profileData.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;
			locationData.LongestSurvivedMillis = TimeSpan.FromSeconds(gameModel.GameTime.Value).TotalMilliseconds;

			while (profileData.Experience >= profileData.MaxExperience)
			{
				profileData.Level++;
				profileData.Experience -= profileData.MaxExperience;

				var config = _profileConfigProvider.Get(profileData.Level);
				profileData.MaxExperience = config?.ExperienceToLevelUp ?? 1_000_000_000;
				profileData.BalanceData.Energy += RoguelikeConstants.Meta.INCREASE_ENERGY_PER_GAINED_LEVEL;
			}

			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				await _restApiClient.PostGameSessionClose(new GameSessionPostBody()
				{
					surviveMills = (int)locationData.LongestSurvivedMillis
				});
			}
			
			profileModel.Update(profileData);
			locationsModel.Update(locationsData);
			
			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.MainMenu);

			_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
			
			await _sceneService.UnloadAdditiveSceneByNameAsync(_locationConfig.SceneName);
		}

		public class Factory : PlaceholderFactory<ILocationConfig, SingleMatch>
		{
		}
	}
}