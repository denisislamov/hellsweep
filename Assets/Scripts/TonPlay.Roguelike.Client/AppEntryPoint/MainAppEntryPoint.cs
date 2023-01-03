using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.SceneService.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.AppEntryPoint
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