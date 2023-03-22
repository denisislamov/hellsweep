using System;
using System.Collections.Generic;
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
		private readonly ILocationConfigProvider _locationConfigProvider;

		public SingleMatch(
			ILocationConfig locationConfig,
			ISceneService sceneService,
			IUIService uiService,
			IGameModelProvider gameModelProvider,
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			ILocationConfigProvider locationConfigProvider)
		{
			_sceneService = sceneService;
			_uiService = uiService;
			_gameModelProvider = gameModelProvider;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_locationConfig = locationConfig;
			_restApiClient = restApiClient;
			_locationConfigProvider = locationConfigProvider;
		}
		
		public async UniTask<bool> Launch()
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var data = balanceModel.ToData();

			if (data.Energy < RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE)
			{
				return false;
			}

			data.Energy -= RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE;

			balanceModel.Update(data);

			var requestBody = new OpenGameSessionPostBody()
			{
				pve = true,
				locationId = _locationConfig.Id
			};
			
			await _restApiClient.PostGameSession(requestBody); 
			
			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				await _restApiClient.PostGameSessionClose(new CloseGameSessionPostBody()
				{
					surviveMills = 0
				});
			}

			await _restApiClient.PostGameSession(requestBody); // TODO - add PVE variable
			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(_locationConfig.SceneName);

			return true;
		}

		public async UniTask<GameSessionResponse> FinishSession(IMatchResult matchResult)
		{
			var response = new GameSessionResponse();
			
			var metaGameModel = _metaGameModelProvider.Get();
			var gameModel = _gameModelProvider.Get();

			var locationsModel = metaGameModel.LocationsModel;
			var locationsData = locationsModel.ToData();
			
			var profileModel = metaGameModel.ProfileModel;
			var profileData = profileModel.ToData();

			UpdateCurrentLocationTimeMillis(locationsData, gameModel);
			UpdatePlayerProfile(profileData, gameModel);

			if (matchResult.MatchResultType == MatchResultType.Win)
			{
				UnlockNextLocation(locationsData);
			}

			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				response = await _restApiClient.PostGameSessionClose(new CloseGameSessionPostBody()
				{
					surviveMills = Convert.ToInt64(locationsData.Locations[_locationConfig.ChapterIdx].LongestSurvivedMillis),
					coins = Convert.ToInt64(gameModel.PlayerModel.MatchProfileGainModel.Gold.Value)
				});

				if (response.rewardSummary != null)
				{
					var gainData = gameModel.PlayerModel.MatchProfileGainModel.ToData();
					gainData.Gold += response.rewardSummary.coin; // TODO check it
					gainData.ProfileExperience = response.rewardSummary.xp; // TODO check it as well

					var chests = response.rewardSummary.chests;
					if (chests != null && chests.Count > 0)
					{
						gainData.ChestsId = new List<string>(chests.Count);
						for (var index = 0; index < chests.Count; index++)
						{
							gainData.ChestsId[index] = chests[index].id;
						}
					}
					
					gameModel.PlayerModel.MatchProfileGainModel.Update(gainData);
				}
			}
			
			profileModel.Update(profileData);
			locationsModel.Update(locationsData);

			return gameSessionResponse;
		}
		
		public async UniTask Finish()
		{
			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.MainMenu);

			_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
			
			await _sceneService.UnloadAdditiveSceneByNameAsync(_locationConfig.SceneName);
		}

		private void UpdatePlayerProfile(ProfileData profileData, IGameModel gameModel)
		{
			profileData.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
			profileData.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;

			while (profileData.Experience >= profileData.MaxExperience)
			{
				profileData.Level++;
				profileData.Experience -= profileData.MaxExperience;

				var config = _profileConfigProvider.Get(profileData.Level);
				profileData.MaxExperience = config?.ExperienceToLevelUp ?? 1_000_000_000;
				profileData.BalanceData.Energy += RoguelikeConstants.Meta.INCREASE_ENERGY_PER_GAINED_LEVEL;
			}
		}
		
		private void UpdateCurrentLocationTimeMillis(LocationsData locationsData, IGameModel gameModel)
		{
			if (!locationsData.Locations.ContainsKey(_locationConfig.ChapterIdx))
			{
				locationsData.Locations.Add(
					_locationConfig.ChapterIdx,
					new LocationData()
					{
						ChapterIdx = _locationConfig.ChapterIdx
					});
			}

			var locationData = locationsData.Locations[_locationConfig.ChapterIdx];

			var currentMatchTimeMillis = TimeSpan.FromSeconds(gameModel.GameTime.Value).TotalMilliseconds;

			if (locationData.LongestSurvivedMillis > currentMatchTimeMillis)
			{
				return;
			}
			
			locationData.LongestSurvivedMillis = currentMatchTimeMillis;
		}

		private void UnlockNextLocation(LocationsData locationsData)
		{
			var nextLocation = _locationConfigProvider.Get(_locationConfig.ChapterIdx + 1);

			if (nextLocation is null)
			{
				return;
			}
			
			if (!locationsData.Locations.ContainsKey(nextLocation.ChapterIdx))
			{
				locationsData.Locations.Add(
					nextLocation.ChapterIdx,
					new LocationData()
					{
						ChapterIdx = nextLocation.ChapterIdx
					});
			}

			var nextLocationData = locationsData.Locations[nextLocation.ChapterIdx];
			nextLocationData.Unlocked = true;
		}

		public class Factory : PlaceholderFactory<ILocationConfig, SingleMatch>
		{
		}
	}
}