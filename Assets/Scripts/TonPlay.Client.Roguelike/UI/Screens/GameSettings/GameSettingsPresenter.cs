using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
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
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		
		private GamePropertiesResponse.JsonData.GameSettings _gameSettingCached;
		
		// TODO IGameSettingsScreen - to ScreenContext
		public GameSettingsPresenter(
			IGameSettingsView view,
			IGameSettingsScreenContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			SliderPresenter.Factory sliderPresenterFactory,
			TogglePresenter.Factory togglePresenterFactory,
			IUIService uiService,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_sliderPresenterFactory = sliderPresenterFactory;
			_togglePresenterFactory = togglePresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			
			_uiService = uiService;
			_restApiClient = restApiClient;

			// TODO add save and load data to meta game settings
		}

		public override async void Show()
		{
			_gameSettingCached = new GamePropertiesResponse.JsonData.GameSettings();
			base.Show();
			
			AddButtonsPresenter();
			AddSlidersPresenter();
			AddTogglesPresenter();
			SetViewValues();
		}

		private void SetViewValues()
		{
			var data = _metaGameModelProvider.Get().GameSettingsModel.ToData();
			
			View.MusicSlider.SetSliderValue(data.MusicVolume);
			View.SoundSlider.SetSliderValue(data.SoundsVolume);
			View.ScreenGameStickToggle.SetToggleValue(data.ScreenGameStick);
			View.VisualizeDamageToggle.SetToggleValue(data.VisualizeDamage);
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
			_gameSettingCached.VisualizeDamage = value;
			Debug.LogFormat("OnVisualizeDamageToggleValueChanged: {0}", value);
		}

		private void OnScreenGameStickToggleValueChanged(bool value)
		{
			_gameSettingCached.ScreenGameStick = value;
			Debug.LogFormat("OnScreenGameStickToggleValueChanged: {0}", value);
		}


		private void OnMusicSliderValueChanged(float value)
		{
			_gameSettingCached.MusicVolume = value;
			Debug.LogFormat("OnMusicSliderValueChanged: {0}", value);
		}

		private void OnSoundValueChanged(float value)
		{
			_gameSettingCached.SoundsVolume = value;
			Debug.LogFormat("OnSoundValueChanged: {0}", value);
		}

		private async void ApplyButtonClickHandler()
		{
			UpdateGameSettingsModel();
			await PostGameSettingsToServer();

			Debug.Log("ApplyButtonClickHandler");
			Hide();
		}

		private async Task PostGameSettingsToServer()
		{
			var gameSettingResponse = new GamePropertiesResponse
			{
				jsonData =
				{
					gameSettings = _gameSettingCached
				}
			};
			var gamePropertiesResponse = await _restApiClient.PostGameProperties(gameSettingResponse);
			if (gamePropertiesResponse == null)
			{
				Debug.LogWarning("Can't set GamePropertiesResponse to server");
			}
		}

		private void UpdateGameSettingsModel()
		{
			var data = _metaGameModelProvider.Get().GameSettingsModel.ToData();

			data.MusicVolume = _gameSettingCached.MusicVolume;
			data.SoundsVolume = _gameSettingCached.SoundsVolume;
			data.ScreenGameStick = _gameSettingCached.ScreenGameStick;
			data.VisualizeDamage = _gameSettingCached.VisualizeDamage;

			_metaGameModelProvider.Get().GameSettingsModel.Update(data);
		}

		private void CloseButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
		}

		internal class Factory : PlaceholderFactory<IGameSettingsView, IGameSettingsScreenContext, GameSettingsPresenter>
		{

		}
	}
}