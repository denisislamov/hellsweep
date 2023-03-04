using System;
using System.Collections.Generic;
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
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
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
		private readonly RewardItemCollectionPresenter.Factory _rewardItemCollectionPresenterFactory;
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
			ILocationConfigStorage locationConfigStorage,
			RewardItemCollectionPresenter.Factory rewardItemCollectionPresenterFactory)
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
			_rewardItemCollectionPresenterFactory = rewardItemCollectionPresenterFactory;

			InitView();
			AddTimerPresenter();
			AddButtonPresenter();
			AddRewardItemCollectionPresenter();
		}

		private void InitView()
		{
			var metaGameModel = _metaGameModelProvider.Get();
			var gameModel = _gameModelProvider.Get();
			var gainModel = gameModel.PlayerModel.MatchProfileGainModel;
			var longestSurvivedMillis =
				metaGameModel.LocationsModel.Locations.ContainsKey(_locationConfigStorage.Current.Id)
					? metaGameModel.LocationsModel.Locations[_locationConfigStorage.Current.Id].LongestSurvivedMillis.Value
					: 0d;

			var longestSurvivedTimeSpan = TimeSpan.FromMilliseconds(longestSurvivedMillis);
			var currentSurvivedTimeSpan = TimeSpan.FromSeconds(gameModel.GameTime.Value);

			var hasReachedNewRecord = currentSurvivedTimeSpan > longestSurvivedTimeSpan;

			if (hasReachedNewRecord)
			{
				longestSurvivedTimeSpan = currentSurvivedTimeSpan;
			}

			View.SetTitleText("Defeat");
			View.SetBestTimeText($"Best: {longestSurvivedTimeSpan:mm\\:ss}");

			if (hasReachedNewRecord)
			{
				View.SetNewRecordText("NEW RECORD");
			}
			
			View.SetNewRecordActiveState(hasReachedNewRecord);
			
			View.SetLevelTitleText(_locationConfigStorage.Current.Title);
			View.SetKilledEnemiesCountText(string.Format("Enemies defeated: <size=150%>{0}</size>", gameModel.DeadEnemiesCount));
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
		
		private void AddRewardItemCollectionPresenter()
		{
			var rewardList = new List<IRewardData>();
			var gameModel = _gameModelProvider.Get();
			var gainModel = gameModel.PlayerModel.MatchProfileGainModel;

			if (gainModel.Gold.Value > 0)
			{
				rewardList.Add(new RewardData(RoguelikeConstants.Core.Rewards.COINS_ID, gainModel.Gold.Value));
			}
			
			if (gainModel.ProfileExperience.Value > 0)
			{
				rewardList.Add(new RewardData(RoguelikeConstants.Core.Rewards.PROFILE_EXPERIENCE_ID, (int) gainModel.ProfileExperience.Value));
			}
			
			var presenter = _rewardItemCollectionPresenterFactory.Create(
				View.RewardItemCollectionView, 
				new RewardItemCollectionContext(rewardList));
			
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
					new LocationData() {Id = _locationConfigStorage.Current.Id});
			}

			var locationData = locationsData.Locations[_locationConfigStorage.Current.Id];

			var profileModel = metagameModel.ProfileModel;
			var gameModel = _gameModelProvider.Get();

			var profileData = profileModel.ToData();

			profileData.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
			profileData.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;
			locationData.LongestSurvivedMillis = TimeSpan.FromSeconds(gameModel.GameTime.Value).TotalMilliseconds;

			while (profileData.Experience >= profileData.MaxExperience)
			{
				profileData.Level++;
				profileData.Experience -= profileData.MaxExperience;

				var config = _profileConfigProvider.Get(profileData.Level);
				profileData.MaxExperience = config?.ExperienceToLevelUp ?? 1_000_000_000;
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