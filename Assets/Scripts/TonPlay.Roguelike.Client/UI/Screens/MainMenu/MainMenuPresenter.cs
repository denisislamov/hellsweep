using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.SceneService.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.Game;
using TonPlay.Roguelike.Client.UI.Screens.Game.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UniRx;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.Screens.MainMenu
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
			_sceneService.LoadSingleSceneByNameAsync(SceneName.Level_Sand).ContinueWith(() =>
			{
				_uiService.Close(Context.Screen);
				_uiService.Open<GameScreen, IGameScreenContext>(new GameScreenContext());
			});
		}

		internal class Factory : PlaceholderFactory<IMainMenuView, IMainMenuScreenContext, MainMenuPresenter>
		{
		}
	}
}