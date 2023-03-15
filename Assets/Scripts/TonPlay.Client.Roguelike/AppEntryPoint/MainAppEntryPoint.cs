using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.AppEntryPoint
{
	public class MainAppEntryPoint : SceneLoadingAppEntryPoint
	{
		private readonly IUIService _uiService;
		private readonly IRestApiClient _restApiClient;
		public MainAppEntryPoint(
			ISceneService sceneService,
			string sceneName,
			IUIService uiService,
			IRestApiClient restApiClient) : base(sceneService, sceneName)
		{
			_uiService = uiService;
			_restApiClient = restApiClient;
		}

		public override async UniTask ProcessEntrance()
		{
			await base.ProcessEntrance();

			//var userSummary = await _restApiClient.GetUserSummary();

			_uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
		}
	}
}