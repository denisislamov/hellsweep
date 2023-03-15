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

            var userSummary = await _restApiClient.GetUserSummary();
            UpdateMetaGameData(userSummary);

            _uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
        }
		
		// TODO maybe move to metaGameModel
        private void UpdateMetaGameData(UserSummaryResponse userSummary)
        {
            var metaGameModel = _metaGameModelProvider.Get();
            var metaGameData = metaGameModel.ToData();

            metaGameData.ProfileData.Experience = userSummary.profile.xp;
            metaGameData.ProfileData.Level = userSummary.profile.level;
            metaGameData.ProfileData.BalanceData.Gold = userSummary.profile.coin;
            metaGameData.ProfileData.BalanceData.MaxEnergy = userSummary.profile.energy;

            metaGameModel.Update(metaGameData);
        }
    }
}