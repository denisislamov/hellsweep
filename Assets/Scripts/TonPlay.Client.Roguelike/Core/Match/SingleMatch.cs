using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Loading;
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
		private readonly IAnalyticsServiceWrapper _analyticsServiceWrapper;
		
		public SingleMatch(
			ILocationConfig locationConfig,
			ISceneService sceneService,
			IUIService uiService,
			IGameModelProvider gameModelProvider,
			IProfileConfigProvider profileConfigProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			ILocationConfigProvider locationConfigProvider,
			IAnalyticsServiceWrapper analyticsServiceWrapper)
		{
			_sceneService = sceneService;
			_uiService = uiService;
			_gameModelProvider = gameModelProvider;
			_profileConfigProvider = profileConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_locationConfig = locationConfig;
			_restApiClient = restApiClient;
			_locationConfigProvider = locationConfigProvider;
			_analyticsServiceWrapper = analyticsServiceWrapper;
		}
		
		public async UniTask<bool> Launch()
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var data = balanceModel.ToData();

			if (data.Energy < RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE)
			{
				return false;
			}
			
			var loadingScreen = _uiService.Open<LoadingScreen, IScreenContext>(ScreenContext.Empty, false, new LoadingScreenLayer());

			data.Energy -= RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE;

			balanceModel.Update(data);

			var requestBody = new OpenGameSessionPostBody()
			{
				pve = true,
				locationId = _locationConfig.Id
			};
			
			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				await _restApiClient.PostGameSessionClose(new CloseGameSessionPostBody()
				{
					surviveMills = 0
				});
			}

			var openSessionResponse = await _restApiClient.PostGameSession(requestBody);
			if (openSessionResponse == null)
			{
				_uiService.Close(loadingScreen);
				return false;
			}
			
			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(_locationConfig.SceneName);
			
			_uiService.Close(loadingScreen);

			return true;
		}

		public async UniTask<GameSessionResponse> FinishSession(IMatchResult matchResult)
		{
			var response = new Response<GameSessionResponse>();
			
			var metaGameModel = _metaGameModelProvider.Get();
			var gameModel = _gameModelProvider.Get();

			var locationsModel = metaGameModel.LocationsModel;
			var locationsData = locationsModel.ToData();
			
			var profileModel = metaGameModel.ProfileModel;
			var profileData = profileModel.ToData();

			var gameSessionResponse = await _restApiClient.GetGameSession();
			if (gameSessionResponse != null)
			{
				var surviveTime = TimeSpan.FromSeconds(gameModel.GameTimeInSeconds.Value);

				if (HasDiedOnBossFight(matchResult, gameModel))
				{
					surviveTime = SetSurviveTimeToPreviousMinuteLastSecond(gameModel);
				}

				response = await _restApiClient.PostGameSessionClose(new CloseGameSessionPostBody()
				{
					surviveMills = Convert.ToInt64(surviveTime.TotalMilliseconds),
					coins = Convert.ToInt64(gameModel.PlayerModel.MatchProfileGainModel.Gold.Value)
				});

				if (response.response.rewardSummary != null)
				{
					UpdateGainModelWithResponse(gameModel, response.response);
				}
			}
			
			UpdatePlayerProfile(profileData, gameModel);
			profileModel.Update(profileData);
			
			UpdateCurrentLocationTimeMillis(locationsData, gameModel);
			if (matchResult.MatchResultType == MatchResultType.Win)
			{
				UnlockNextLocation(locationsData);
			}
			locationsModel.Update(locationsData);
			
			await UpdateUserBalance();

			_analyticsServiceWrapper.OnSingleMatchFinishSession(gameModel.PlayerModel.MatchProfileGainModel.Gold.Value);
			
			return gameSessionResponse?.response;
		}

		private async UniTask UpdateUserBalance()
		{
			var userBalanceResponse = await _restApiClient.GetUserBalance();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();

			data.BalanceData.Gold = userBalanceResponse.response.coins;
			data.BalanceData.Energy = userBalanceResponse.response.energy;

			model.Update(data);
		}

		public async UniTask Finish()
		{
			var loadingScreen = _uiService.Open<LoadingScreen, IScreenContext>(ScreenContext.Empty, false, new LoadingScreenLayer());

			await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.MainMenu);

			_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
			
			await _sceneService.UnloadAdditiveSceneByNameAsync(_locationConfig.SceneName);
			
			_uiService.Close(loadingScreen);
		}

		private async void UpdatePlayerProfile(ProfileData profileData, IGameModel gameModel)
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
			
			profileData.InventoryData.Items.AddRange(gameModel.PlayerModel.MatchProfileGainModel.ToData().Items);
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

			var currentMatchTimeMillis = TimeSpan.FromSeconds(gameModel.GameTimeInSeconds.Value).TotalMilliseconds;

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
		
		private static void UpdateGainModelWithResponse(IGameModel gameModel, GameSessionResponse response)
		{
			var gainData = gameModel.PlayerModel.MatchProfileGainModel.ToData();
			gainData.Gold = response.rewardSummary.coin; 
			gainData.ProfileExperience += response.rewardSummary.xp;

			var responseItems = response.rewardSummary.items;
			if (responseItems != null && responseItems.Count > 0)
			{
				var items = new List<InventoryItemData>(responseItems.Count);
				for (var index = 0; index < responseItems.Count; index++)
				{
					items.Add(new InventoryItemData()
					{
						Id = responseItems[index].id,
						ItemId = responseItems[index].itemId,
						DetailId = responseItems[index].itemDetailId
					});
				}

				gainData.Items.Clear();
				gainData.Items.AddRange(items);
			}

			gameModel.PlayerModel.MatchProfileGainModel.Update(gainData);
		}
		
		private static bool HasDiedOnBossFight(IMatchResult matchResult, IGameModel gameModel)
		{
			return matchResult.MatchResultType == MatchResultType.Lose && 
				   gameModel.BossModel.Exists.Value &&
				   gameModel.BossModel.Health.Value > 0;
		}
		
		private static TimeSpan SetSurviveTimeToPreviousMinuteLastSecond(IGameModel gameModel)
		{
			var surviveTime = TimeSpan.FromMinutes(Math.Round(TimeSpan.FromSeconds(gameModel.GameTimeInSeconds.Value).TotalMinutes) - 1) 
							  + TimeSpan.FromSeconds(59);
			var gameData = gameModel.ToData();
			gameData.GameTimeInSeconds = surviveTime.TotalSeconds;
			gameModel.Update(gameData);
			return surviveTime;
		}

		public class Factory : PlaceholderFactory<ILocationConfig, SingleMatch>
		{
		}
	}
}