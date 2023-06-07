using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Samples.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Sliders;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using TonPlay.Client.Roguelike.UI.Toggles;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Samples
{
    public class SimplePopupView : View, ISimplePopupView
    {
        [SerializeField] private ButtonView _closeButton;

        public IButtonView CloseButton => _closeButton;
    }
}
