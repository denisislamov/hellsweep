using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
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
		private const int ENERGY_TO_START = 5;

		private readonly IUIService _uiService;
		private readonly ISceneService _sceneService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ProfileBarPresenter.Factory _profileBarPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		
		public MainMenuPresenter(
			IMainMenuView view, 
			IMainMenuScreenContext context,
			IUIService uiService,
			ISceneService sceneService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider) 
			: base(view, context)
		{
			_uiService = uiService;
			_sceneService = sceneService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;

			AddNestedButtonPresenter();
			AddNestedProfileBarPresenter();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			
			base.Dispose();
		}

		private void AddNestedButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.PlayButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnPlayButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void AddNestedProfileBarPresenter()
		{
			var presenter = _profileBarPresenterFactory.Create(View.ProfileBarView, new ProfileBarContext());
			
			Presenters.Add(presenter);
		}

		private void OnPlayButtonClickHandler()
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var data = balanceModel.ToData();

			if (data.Energy < ENERGY_TO_START)
			{
				return;
			}

			data.Energy -= ENERGY_TO_START;
			
			balanceModel.Update(data);
			
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