using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces
{
    public interface IGameSettingsView : IView
    {
        IButtonView ApplyButton { get; }
        IButtonView CloseButton { get; }
        
        ISliderView SoundSlider { get; }
        ISliderView MusicSlider { get; }
        
        IToggleView ScreenGameStickToggle { get; }
        IToggleView VisualizeDamageToggle { get; } 

        void SetSoundSliderValue(float value);
        void SetMusicSliderValue(float value);
        
        void SetScreenGameStickToggleValue(bool value);
        void SetVisualizeDamageToggleValue(bool value);
    }
}