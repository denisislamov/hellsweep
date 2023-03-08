using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Match;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	internal class MainMenuPresenter : Presenter<IMainMenuView, IMainMenuScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly ISceneService _sceneService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ProfileBarPresenter.Factory _profileBarPresenterFactory;
		private readonly LocationSliderPresenter.Factory _locationSliderPresenterFactory;
		private readonly IMatchLauncher _matchLauncher;
		private readonly ILocationConfigStorageSelector _locationConfigStorageSelector;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		private bool _launchingMatch;

		public MainMenuPresenter(
			IMainMenuView view,
			IMainMenuScreenContext context,
			IUIService uiService,
			ISceneService sceneService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			LocationSliderPresenter.Factory locationSliderPresenterFactory,
			ILocationConfigStorageSelector locationConfigStorageSelector,
			IMatchLauncher matchLauncher)
			: base(view, context)
		{
			_uiService = uiService;
			_sceneService = sceneService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_locationSliderPresenterFactory = locationSliderPresenterFactory;
			_matchLauncher = matchLauncher;
			_locationConfigStorageSelector = locationConfigStorageSelector;

			AddNestedButtonPresenter();
			AddNestedProfileBarPresenter();
			AddNestedLocationSliderPresenter(locationConfigStorageSelector);
		}

		private void AddNestedLocationSliderPresenter(ILocationConfigStorageSelector selector)
		{
			var presenter = _locationSliderPresenterFactory.Create(View.LocationSliderView, new LocationSliderContext(selector));

			Presenters.Add(presenter);
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

		private async void OnPlayButtonClickHandler()
		{
			if (_launchingMatch)
			{
				return;
			}

			_launchingMatch = true;

			try
			{
				await _matchLauncher.Launch(MatchType.OfflineSingle, _locationConfigStorageSelector.Current);

				await _sceneService.UnloadAdditiveSceneByNameAsync(SceneName.MainMenu);

				_uiService.Close(Context.Screen);
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
			}
			finally
			{
				_launchingMatch = false;
			}
		}

		internal class Factory : PlaceholderFactory<IMainMenuView, IMainMenuScreenContext, MainMenuPresenter>
		{
		}
	}
}