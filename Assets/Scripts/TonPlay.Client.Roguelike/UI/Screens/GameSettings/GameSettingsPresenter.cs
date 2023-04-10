using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
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

        // TODO IGameSettingsScreen - to ScreenContext
        public GameSettingsPresenter(
            IGameSettingsView view,
            IGameSettingsScreenContext context,
            IButtonPresenterFactory buttonPresenterFactory,
            SliderPresenter.Factory sliderPresenterFactory,
            IUIService uiService,
            IRestApiClient restApiClient)
            : base(view, context)
        {
            _buttonPresenterFactory = buttonPresenterFactory;
            _sliderPresenterFactory = sliderPresenterFactory;
            _uiService = uiService;
            _restApiClient = restApiClient;

            // TODO add save and load data to meta game settings
        }

        public override void Show()
        {
            // TODO add load data to meta game settings
            // TODO add load data from rest api
            base.Show();

            AddButtonsPresenter();
            AddSlidersPresenter();
            AddTogglesPresenter();
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
            ISliderContext musicSliderContext = new SliderViewContext();
            musicSliderContext.OnValueChanged = OnSoundValueChanged;

            var presenter = _sliderPresenterFactory.Create(View.MusicSlider, musicSliderContext);
            Presenters.Add(presenter);

            ISliderContext soundSliderContext = new SliderViewContext();
            soundSliderContext.OnValueChanged = OnSoundValueChanged;

            presenter = _sliderPresenterFactory.Create(View.SoundSlider, soundSliderContext);
            Presenters.Add(presenter);

            // TODO - ask who remove func from listener
        }

        private void AddTogglesPresenter()
        {
            var presenter = _togglePresenterFactory.Create(View.ScreenGameStickToggle, new ToggleViewContext()
            {
                OnValueChanged = OnScreenGameStickToggleValueChanged
            });
            Presenters.Add(presenter);
            
            presenter = _togglePresenterFactory.Create(View.VisualizeDamageToggle, new ToggleViewContext()
            {
                OnValueChanged = OnVisualizeDamageToggleValueChanged
            });
            Presenters.Add(presenter);
        }

        private void OnVisualizeDamageToggleValueChanged(bool value)
        {
            // TODO Switch game stick view and visual damage
            Debug.LogFormat("OnVisualizeDamageToggleValueChanged: {0}", value);
        }

        private void OnScreenGameStickToggleValueChanged(bool value)
        {
            // TODO Switch game stick view and visual damage
            Debug.LogFormat("OnScreenGameStickToggleValueChanged: {0}", value);
        }


        private void OnMusicSliderValueChanged(float value)
        {
            Debug.LogFormat("OnMusicSliderValueChanged: {0}", value);
        }

        private void OnSoundValueChanged(float value)
        {
            Debug.LogFormat("OnSoundValueChanged: {0}", value);
        }
        
        private void ApplyButtonClickHandler()
        {
            // TODO add save data to meta game settings
            // TODO add save data from rest api
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