using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
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
			IProfileConfigProvider profileConfigProvider)
			: base(view, context)
		{
			_timerPresenterFactory = timerPresenterFactory;
			_gameModelProvider = gameModelProvider;
			_metaGameModelProvider = metaGameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_sceneService = sceneService;
			_uiService = uiService;
			_profileConfigProvider = profileConfigProvider;

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
			var profileModel = _metaGameModelProvider.Get().ProfileModel;
			var gameModel = _gameModelProvider.Get();

			var data = profileModel.ToData();
			data.BalanceData.Gold += gameModel.PlayerModel.MatchProfileGainModel.Gold.Value;
			data.Experience += gameModel.PlayerModel.MatchProfileGainModel.ProfileExperience.Value;

			while(data.Experience >= data.MaxExperience)
			{
				data.Level++;
				data.Experience -= data.MaxExperience;

				var config = _profileConfigProvider.Get(data.Level);
				data.MaxExperience = config.ExperienceToLevelUp;
				data.BalanceData.Energy += 5;
			}
			
			profileModel.Update(data);
		}

		internal class Factory : PlaceholderFactory<IDefeatGameView, IDefeatGameScreenContext, DefeatGamePresenter>
		{
		}
	}
}