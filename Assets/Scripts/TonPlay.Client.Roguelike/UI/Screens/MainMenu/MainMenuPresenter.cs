using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Match;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
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
		private readonly NavigationMenuPresenter.Factory _navigationMenuPresenterFactory;
		private readonly IMatchLauncher _matchLauncher;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ILocationConfigStorageSelector _locationConfigStorageSelector;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		private bool _launchingMatch;
		private ReactiveProperty<bool> _playButtonLockState;

		public MainMenuPresenter(
			IMainMenuView view,
			IMainMenuScreenContext context,
			IUIService uiService,
			ISceneService sceneService,
			IButtonPresenterFactory buttonPresenterFactory,
			ProfileBarPresenter.Factory profileBarPresenterFactory,
			LocationSliderPresenter.Factory locationSliderPresenterFactory,
			NavigationMenuPresenter.Factory navigationMenuPresenterFactory,
			ILocationConfigStorageSelector locationConfigStorageSelector,
			IMatchLauncher matchLauncher,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_uiService = uiService;
			_sceneService = sceneService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_profileBarPresenterFactory = profileBarPresenterFactory;
			_locationSliderPresenterFactory = locationSliderPresenterFactory;
			_navigationMenuPresenterFactory = navigationMenuPresenterFactory;
			_matchLauncher = matchLauncher;
			_metaGameModelProvider = metaGameModelProvider;
			_locationConfigStorageSelector = locationConfigStorageSelector;
			_restApiClient = restApiClient;

			AddNestedButtonPresenter();
			AddNestedProfileBarPresenter();
			AddNestedLocationSliderPresenter(locationConfigStorageSelector);
			AddNavigationMenuPresenter();
			AddCurrentLocationSubscription();

			AddUserProfileUpdateScheduler();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}

		private void AddUserProfileUpdateScheduler()
		{
			Observable
			   .Interval(TimeSpan.FromSeconds(60)) // TODO - move to some config
			   .Subscribe(_ => UpdateUserProfile())
			   .AddTo(_compositeDisposables);
		}

		private async void UpdateUserProfile()
		{
			var userBalanceResponse = await _restApiClient.GetUserBalance();

			var metaGameModel = _metaGameModelProvider.Get();
			var model = metaGameModel.ProfileModel;
			var data = metaGameModel.ProfileModel.ToData();

			data.BalanceData.Gold = userBalanceResponse.response.coin;
			data.BalanceData.Energy = userBalanceResponse.response.energy;

			model.Update(data);
		}

		private void AddNavigationMenuPresenter()
		{
			var presenter = _navigationMenuPresenterFactory.Create(
				View.NavigationMenuView, 
				new NavigationMenuContext(NavigationMenuTabName.MainMenu){ Screen = Context.Screen });
			
			Presenters.Add(presenter);
		}

		private void AddCurrentLocationSubscription()
		{
			_locationConfigStorageSelector
			   .Current
			   .Subscribe(config =>
				{
					var metaGameModel = _metaGameModelProvider.Get();

					if (config.AlreadyUnlocked)
					{
						_playButtonLockState.SetValueAndForceNotify(false);
						return;
					}

					if (!metaGameModel.LocationsModel.Locations.ContainsKey(config.ChapterIdx))
					{
						_playButtonLockState.SetValueAndForceNotify(true);
						return;
					}

					var model = _metaGameModelProvider.Get().LocationsModel.Locations[config.ChapterIdx];
					_playButtonLockState.SetValueAndForceNotify(!model.Unlocked.Value);
				})
			   .AddTo(_compositeDisposables);
		}

		private void AddNestedLocationSliderPresenter(ILocationConfigStorageSelector selector)
		{
			var presenter = _locationSliderPresenterFactory.Create(View.LocationSliderView, new LocationSliderContext(selector));

			Presenters.Add(presenter);
		}

		private void AddNestedButtonPresenter()
		{
			_playButtonLockState = new ReactiveProperty<bool>();

			var presenter = _buttonPresenterFactory.Create(View.PlayButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnPlayButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_playButtonLockState)));

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
				if (await _matchLauncher.Launch(MatchType.OfflineSingle, _locationConfigStorageSelector.Current.Value))
				{
					await _sceneService.UnloadAdditiveSceneByNameAsync(SceneName.MainMenu);

					_uiService.Close(Context.Screen);
				}
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