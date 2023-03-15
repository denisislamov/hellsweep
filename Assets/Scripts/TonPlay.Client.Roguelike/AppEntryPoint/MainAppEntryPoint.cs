using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.AppEntryPoint
{
	public class MainAppEntryPoint : SceneLoadingAppEntryPoint
	{
		private readonly IUIService _uiService;
		private readonly IRestApiClient _restApiClient;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		public MainAppEntryPoint(
			ISceneService sceneService,
			string sceneName,
			IUIService uiService,
			IRestApiClient restApiClient,
			IMetaGameModelProvider metaGameModelProvider) : base(sceneService, sceneName)
		{
			_uiService = uiService;
			_restApiClient = restApiClient;
			_metaGameModelProvider = metaGameModelProvider;
		}

		public override async UniTask ProcessEntrance()
        {
            await base.ProcessEntrance();

            var userSummaryResponse = await _restApiClient.GetUserSummary();
            var infoLevelAllResponse = await _restApiClient.GetInfoLevelAll();
            
            UpdateMetaGameData(userSummaryResponse, infoLevelAllResponse);

            _uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
        }
		
		// TODO maybe move to metaGameModel or somewhere else.
        private void UpdateMetaGameData(UserSummaryResponse userSummary, 
									    InfoLevelAllResponse infoLevelAllResponse)
        {
            var metaGameModel = _metaGameModelProvider.Get();
            var metaGameData = metaGameModel.ToData();

            metaGameData.ProfileData.Experience = userSummary.profile.xp;
            metaGameData.ProfileData.Level = userSummary.profile.level;
            metaGameData.ProfileData.BalanceData.Gold = userSummary.profile.coin;
            metaGameData.ProfileData.BalanceData.Energy = 0; // TODO not sure about this. Islamov Denis.
            metaGameData.ProfileData.BalanceData.MaxEnergy = userSummary.profile.energy;

            metaGameData.UserLevelsInfoData = new UserLevelsInfoData
            {
	            Levels = new UserLevelInfoData[infoLevelAllResponse.items.Count].ToList()
            };

            for (var index = 0; index < infoLevelAllResponse.items.Count; index++)
            {
	            var items = infoLevelAllResponse.items[index];
	            metaGameData.UserLevelsInfoData.Levels[index].Level = items.level;
	            metaGameData.UserLevelsInfoData.Levels[index].Coin = items.coin;
	            metaGameData.UserLevelsInfoData.Levels[index].Xp = items.xp;
            }
            
            metaGameData.ProfileData.MaxExperience = metaGameData.UserLevelsInfoData.Levels
	            .FirstOrDefault(x => x.Level == metaGameData.ProfileData.Level + 1)?.Xp ?? 0;
            
            metaGameModel.Update(metaGameData);
        }
    }
}