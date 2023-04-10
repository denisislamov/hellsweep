using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Sliders;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using TonPlay.Client.Roguelike.UI.Toggles;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.GameSettings
{
    public class GameSettingsView : View, IGameSettingsView
    {
        [SerializeField]
        private ButtonView _applyButton;
        [SerializeField]
        private ButtonView _closeButton;

        public IButtonView ApplyButton => _applyButton;
        public IButtonView CloseButton => _closeButton;
        
        [SerializeField]
        private SliderView _soundSlider;
        [SerializeField]
        private SliderView _musicSlider;
        
        public ISliderView SoundSlider => _soundSlider;
        public ISliderView MusicSlider => _musicSlider;
        
        [SerializeField]
        private ToggleView _screenGameStickToggle;
        [SerializeField]
        private ToggleView _visualizeDamageToggle;
        
        public IToggleView ScreenGameStickToggle => _screenGameStickToggle;
        public IToggleView VisualizeDamageToggle => _visualizeDamageToggle;
        
        public void SetSoundSliderValue(float value)
        {
            _soundSlider.SetSliderValue(value);
        }

        public void SetMusicSliderValue(float value)
        {
            _musicSlider.SetSliderValue(value);
        }

        public void SetScreenGameStickToggleValue(bool value)
        {
            _screenGameStickToggle.SetToggleValue(value);
        }

        public void SetVisualizeDamageToggleValue(bool value)
        {
            _visualizeDamageToggle.SetToggleValue(value);
        }
    }
}
