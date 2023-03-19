using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Match
{
	// public class OfflineSingleMatch : IMatch
	// {
	// 	private readonly ISceneService _sceneService;
	// 	private readonly IUIService _uiService;
	// 	private readonly IGameModelProvider _gameModelProvider;
	// 	private readonly IProfileConfigProvider _profileConfigProvider;
	// 	private readonly IMetaGameModelProvider _metaGameModelProvider;
	// 	private readonly ILocationConfig _locationConfig;
	// 	
	// 	public OfflineSingleMatch(
	// 		ILocationConfig locationConfig,
	// 		ISceneService sceneService,
	// 		IUIService uiService,
	// 		IGameModelProvider gameModelProvider,
	// 		IProfileConfigProvider profileConfigProvider,
	// 		IMetaGameModelProvider metaGameModelProvider)
	// 	{
	// 		_sceneService = sceneService;
	// 		_uiService = uiService;
	// 		_gameModelProvider = gameModelProvider;
	// 		_profileConfigProvider = profileConfigProvider;
	// 		_metaGameModelProvider = metaGameModelProvider;
	// 		_locationConfig = locationConfig;
	// 	}
	// 	
	// 	public async UniTask Launch()
	// 	{
	// 		var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
	// 		var data = balanceModel.ToData();
	//
	// 		if (data.Energy < RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE)
	// 		{
	// 			return;
	// 		}
	//
	// 		data.Energy -= RoguelikeConstants.Meta.MATCH_ENERGY_PRICE_BASE;
	//
	// 		balanceModel.Update(data);
	//
	// 		await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(_locationConfig.SceneName);
	// 	}
	//
	// 	public async UniTask FinishSession(IMatchResult matchResult)
	// 	{
	// 		var metagameModel = _metaGameModelProvider.Get();
	//
	// 		var locationsModel = metagameModel.LocationsModel;
	// 		var locationsData = locationsModel.ToData();
	//
	// 		if (!locationsData.Locations.ContainsKey(_locationConfig.Id))
	// 		{
	// 			locationsData.Locations.Add(
	// 				_locationConfig.Id,
	// 				new LocationData() {Id = _locationConfig.Id});
	// 		}
	//
	// 		var locationData = locationsData.Locations[_locationConfig.Id];
	//
	// 		var profileModel = metagameModel.ProfileModel;
	// 		var gameModel = _gameModelProvider.Get();
	//
	// 		var profileData = profileModel.ToData();
	//
	// 		profileData.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
	// 		profileData.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;
	// 		locationData.LongestSurvivedMillis = TimeSpan.FromSeconds(gameModel.GameTime.Value).TotalMilliseconds;
	//
	// 		while (profileData.Experience >= profileData.MaxExperience)
	// 		{
	// 			profileData.Level++;
	// 			profileData.Experience -= profileData.MaxExperience;
	//
	// 			var config = _profileConfigProvider.Get(profileData.Level);
	// 			profileData.MaxExperience = config?.ExperienceToLevelUp ?? 1_000_000_000;
	// 			profileData.BalanceData.Energy += RoguelikeConstants.Meta.INCREASE_ENERGY_PER_GAINED_LEVEL;
	// 		}
	//
	// 		profileModel.Update(profileData);
	// 		locationsModel.Update(locationsData);
	// 		
	// 		await _sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.MainMenu);
	//
	// 		_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
	// 		
	// 		await _sceneService.UnloadAdditiveSceneByNameAsync(_locationConfig.SceneName);
	// 	}
	//
	// 	public class Factory : PlaceholderFactory<ILocationConfig, OfflineSingleMatch>
	// 	{
	// 	}
	// }
}