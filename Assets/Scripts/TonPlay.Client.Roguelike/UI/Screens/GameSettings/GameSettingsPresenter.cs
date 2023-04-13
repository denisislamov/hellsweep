using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Sliders;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using TonPlay.Client.Roguelike.UI.Toggles;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.GameSettings
{
	internal class GameSettingsPresenter : Presenter<IGameSettingsView, IGameSettingsScreenContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly SliderPresenter.Factory _sliderPresenterFactory;
		private readonly TogglePresenter.Factory _togglePresenterFactory;
		private readonly IUIService _uiService;

		// TODO Audio Service
		// TODO Switch game stick view and visual damage

		private readonly IRestApiClient _restApiClient;

		private GamePropertiesResponse.JsonData.GameSettings _gameSetting;
		
		// TODO IGameSettingsScreen - to ScreenContext
		public GameSettingsPresenter(
			IGameSettingsView view,
			IGameSettingsScreenContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			SliderPresenter.Factory sliderPresenterFactory,
			TogglePresenter.Factory togglePresenterFactory,
			IUIService uiService,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_sliderPresenterFactory = sliderPresenterFactory;
			_togglePresenterFactory = togglePresenterFactory;

			_uiService = uiService;
			_restApiClient = restApiClient;

			// TODO add save and load data to meta game settings
		}

		public override async void Show()
		{
			_gameSetting = new GamePropertiesResponse.JsonData.GameSettings();
			// TODO add load data to meta game settings
			// TODO add load data from rest api
			base.Show();
			
			AddButtonsPresenter();
			AddSlidersPresenter();
			AddTogglesPresenter();
			
			var gamePropertiesResponse = await _restApiClient.GetGameProperties();
			if (gamePropertiesResponse != null)
			{
				if (gamePropertiesResponse.response.jsonData != null)
				{
					_gameSetting = gamePropertiesResponse.response.jsonData.gameSettings;
					SetViewValues();
				}
			}
			else
			{
				Debug.LogWarning("Can't get GamePropertiesResponse from server");
			}
		}

		private void SetViewValues()
		{
			View.MusicSlider.SetSliderValue(_gameSetting.MusicVolume);
			View.SoundSlider.SetSliderValue(_gameSetting.SoundsVolume);
			View.ScreenGameStickToggle.SetToggleValue(_gameSetting.ScreenGameStick);
			View.VisualizeDamageToggle.SetToggleValue(_gameSetting.VisualizeDamage);
		}

		public override void Hide()
		{
			base.Hide();
		}

		private void AddButtonsPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.ApplyButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ApplyButtonClickHandler)));

			Presenters.Add(presenter);

			presenter = _buttonPresenterFactory.Create(View.CloseButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(CloseButtonClickHandler)));

			Presenters.Add(presenter);
		}

		private void AddSlidersPresenter()
		{
			var musicSliderContext = new SliderViewContext(
				0.5f, // TODO - replace with current value
				OnMusicSliderValueChanged
			);
			var presenter = _sliderPresenterFactory.Create(View.MusicSlider, musicSliderContext);
			Presenters.Add(presenter);

			var soundSliderContext = new SliderViewContext(
				0.5f, // TODO - replace with current value 
				OnSoundValueChanged
			);
			presenter = _sliderPresenterFactory.Create(View.SoundSlider, soundSliderContext);
			Presenters.Add(presenter);

			// TODO - ask who remove func from listener
		}

		private void AddTogglesPresenter()
		{
			var presenter = _togglePresenterFactory.Create(
				View.ScreenGameStickToggle,
				new ToggleViewContext(
					true, // TODO - replace with current value
					OnScreenGameStickToggleValueChanged
				));
			Presenters.Add(presenter);

			presenter = _togglePresenterFactory.Create(
				View.VisualizeDamageToggle,
				new ToggleViewContext(
					true, // TODO - replace with current value
					OnVisualizeDamageToggleValueChanged
				));
			Presenters.Add(presenter);
		}
		
		private void OnVisualizeDamageToggleValueChanged(bool value)
		{
			// TODO Switch game stick view and visual damage
			_gameSetting.VisualizeDamage = value;
			Debug.LogFormat("OnVisualizeDamageToggleValueChanged: {0}", value);
		}

		private void OnScreenGameStickToggleValueChanged(bool value)
		{
			// TODO Switch game stick view and visual damage
			_gameSetting.ScreenGameStick = value;
			Debug.LogFormat("OnScreenGameStickToggleValueChanged: {0}", value);
		}


		private void OnMusicSliderValueChanged(float value)
		{
			_gameSetting.MusicVolume = value;
			Debug.LogFormat("OnMusicSliderValueChanged: {0}", value);
		}

		private void OnSoundValueChanged(float value)
		{
			_gameSetting.SoundsVolume = value;
			Debug.LogFormat("OnSoundValueChanged: {0}", value);
		}

		private async void ApplyButtonClickHandler()
		{
			// TODO add save data to meta game settings
			// TODO add save data from rest api
			
			var gameSettingResponse = new GamePropertiesResponse();
			gameSettingResponse.jsonData.gameSettings = _gameSetting;
			
			var gamePropertiesResponse = await _restApiClient.PostGameProperties(gameSettingResponse);
			if (gamePropertiesResponse == null)
			{
				Debug.LogWarning("Can't set GamePropertiesResponse to server");
			}

			Debug.Log("ApplyButtonClickHandler");
			Hide();
		}

		private void CloseButtonClickHandler()
		{
			// Reset settings and close
			Debug.Log("CloseButtonClickHandler");
			Hide();
		}

		internal class Factory : PlaceholderFactory<IGameSettingsView, IGameSettingsScreenContext, GameSettingsPresenter>
		{

		}
	}
}