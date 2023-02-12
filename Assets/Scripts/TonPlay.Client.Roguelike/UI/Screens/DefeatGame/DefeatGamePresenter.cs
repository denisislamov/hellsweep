using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.DefeatGame
{
	internal class DefeatGamePresenter : Presenter<IDefeatGameView, IDefeatGameScreenContext>
	{
		private readonly TimerPresenter.Factory _timerPresenterFactory;
		private readonly IGameModelProvider _gameModelProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ISceneService _sceneService;
		private readonly IUIService _uiService;
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly ILocationConfigStorage _locationConfigStorage;
		private bool _loading;

		public DefeatGamePresenter(
			IDefeatGameView view,
			IDefeatGameScreenContext context,
			TimerPresenter.Factory timerPresenterFactory,
			IGameModelProvider gameModelProvider,
			IMetaGameModelProvider metaGameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			ISceneService sceneService,
			IUIService uiService,
			IProfileConfigProvider profileConfigProvider,
			ILocationConfigStorage locationConfigStorage)
			: base(view, context)
		{
			_timerPresenterFactory = timerPresenterFactory;
			_gameModelProvider = gameModelProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_sceneService = sceneService;
			_uiService = uiService;
			_profileConfigProvider = profileConfigProvider;
			_locationConfigStorage = locationConfigStorage;

			InitView();
			AddTimerPresenter();
			AddButtonPresenter();
		}

		private void InitView()
		{
			var metaGameModel = _metaGameModelProvider.Get();
			var gameModel = _gameModelProvider.Get();
			var gainModel = gameModel.PlayerModel.MatchProfileGainModel;

			var time = TimeSpan.FromSeconds(gameModel.GameTime.Value);

			View.SetTitleText("Defeat");
			View.SetBestTimeText($"Best: {time:mm\\:ss}");
			View.SetNewRecordText("New record!");
			View.SetNewRecordActiveState(true);
			View.SetGainedGoldText(string.Format("<sprite name=\"items_spriteatlas_16x16_122\">{0}", gainModel.Gold.Value.ToString()));
			View.SetGainedProfileExperienceText(string.Format("<sprite name=\"items_spriteatlas_16x16_155\">{0}", gainModel.ProfileExperience.Value.ToString(CultureInfo.CurrentCulture)));
			View.SetLevelTitleText("Level 1");
			View.SetKilledEnemiesCountText(string.Format("<sprite name=\"items_spriteatlas_16x16_301\">{0}", gameModel.DeadEnemiesCount));
		}

		private void AddTimerPresenter()
		{
			var gameModel = _gameModelProvider.Get();
			var presenter = _timerPresenterFactory.Create(
				View.TimerView,
				new TimerContext(gameModel.GameTime));

			Presenters.Add(presenter);
		}

		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.ConfirmButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ConfirmButtonClickHandler))
				   .Add(new TextButtonContext("Confirm")));

			Presenters.Add(presenter);
		}

		private void ConfirmButtonClickHandler()
		{
			if (_loading)
			{
				return;
			}

			_loading = true;
			_uiService.CloseAll(new DefaultScreenLayer());

			UpdateProfileModel();
			
			_sceneService.LoadAdditiveSceneWithZenjectByNameAsync(SceneName.MainMenu).ContinueWith(() =>
			{
				_sceneService.UnloadAdditiveSceneByNameAsync(SceneName.Level_Sands);
				_loading = false;
				
				_uiService.Open<MainMenuScreen, IMainMenuScreenContext>(new MainMenuScreenContext());
			});
		}
		
		private void UpdateProfileModel()
		{
			var metagameModel = _metaGameModelProvider.Get();
			
			var locationsModel = metagameModel.LocationsModel;
			var locationsData = locationsModel.ToData();

			if (!locationsData.Locations.ContainsKey(_locationConfigStorage.Current.Id))
			{
				locationsData.Locations.Add(
					_locationConfigStorage.Current.Id, 
					new LocationData(){ Id = _locationConfigStorage.Current.Id });
			}
			
			var locationData = locationsData.Locations[_locationConfigStorage.Current.Id];
			
			var profileModel = metagameModel.ProfileModel;
			var gameModel = _gameModelProvider.Get();

			var profileData = profileModel.ToData();
			
			profileData.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
			profileData.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;
			locationData.LongestSurvivedMillis = TimeSpan.FromSeconds(gameModel.GameTime.Value).TotalMilliseconds;

			while(profileData.Experience >= profileData.MaxExperience)
			{
				profileData.Level++;
				profileData.Experience -= profileData.MaxExperience;

				var config = _profileConfigProvider.Get(profileData.Level);
				profileData.MaxExperience = config.ExperienceToLevelUp;
				profileData.BalanceData.Energy += 5;
			}
			
			profileModel.Update(profileData);
			locationsModel.Update(locationsData);
		}

		internal class Factory : PlaceholderFactory<IDefeatGameView, IDefeatGameScreenContext, DefeatGamePresenter>
		{
		}
	}
}