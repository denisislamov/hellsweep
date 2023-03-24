using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Loading;
using TonPlay.Roguelike.Client.UI.UIService.Layers;

namespace TonPlay.Client.Roguelike.AppEntryPoint
{
	public class MainAppEntryPoint : SceneLoadingAppEntryPoint
	{
		private readonly IUIService _uiService;
		private readonly IUserLoadingService _userLoadingService;
		private readonly IConfigsLoadingService _configsLoadingService;

		public MainAppEntryPoint(
			ISceneService sceneService,
			string sceneName,
			IUIService uiService,
			IUserLoadingService userLoadingService,
			IConfigsLoadingService configsLoadingService) : base(sceneService, sceneName)
		{
			_uiService = uiService;
			_userLoadingService = userLoadingService;
			_configsLoadingService = configsLoadingService;
		}

		public override async UniTask ProcessEntrance()
        {
            await base.ProcessEntrance();	
			
			var loadingScreen = _uiService.Open<LoadingScreen, IScreenContext>(ScreenContext.Empty, false, new LoadingScreenLayer());
			
			await _configsLoadingService.Load();

			await _userLoadingService.Load();

            _uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
			
			_uiService.Close(loadingScreen);
        }
    }
}