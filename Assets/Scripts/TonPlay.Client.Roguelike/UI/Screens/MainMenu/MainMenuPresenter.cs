using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	internal class MainMenuPresenter : Presenter<IMainMenuView, IMainMenuScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly ISceneService _sceneService;
		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		
		public MainMenuPresenter(
			IMainMenuView view, 
			IMainMenuScreenContext context,
			IUIService uiService,
			ISceneService sceneService) 
			: base(view, context)
		{
			_uiService = uiService;
			_sceneService = sceneService;

			AddNestedPresenter();
		}

		public override void Hide()
		{
			_compositeDisposables.Dispose();
			base.Hide();
		}

		private void AddNestedPresenter()
		{
			View.PlayButton.OnClickAsObservable().Subscribe(_ => OnPlayButtonClickHandler()).AddTo(_compositeDisposables);
		}

		private void OnPlayButtonClickHandler()
		{
			_sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.Level_Sands).ContinueWith(() =>
			{
				_uiService.Close(Context.Screen);
				_sceneService.UnloadAdditiveSceneByNameAsync(SceneName.MainMenu);
			});
		}

		internal class Factory : PlaceholderFactory<IMainMenuView, IMainMenuScreenContext, MainMenuPresenter>
		{
		}
	}
}