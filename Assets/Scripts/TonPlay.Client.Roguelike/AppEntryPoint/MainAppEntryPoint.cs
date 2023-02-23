using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.AppEntryPoint
{
	public class MainAppEntryPoint : SceneLoadingAppEntryPoint
	{
		private readonly IUIService _uiService;
		public MainAppEntryPoint(
			ISceneService sceneService,
			string sceneName,
			IUIService uiService) : base(sceneService, sceneName)
		{
			_uiService = uiService;
		}

		public override async UniTask ProcessEntrance()
		{
			await base.ProcessEntrance();

			_uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
		}
	}
}