using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Toggles
{
    public class ToggleViewContext : ScreenContext, IToggleContext
    {
        public Action<bool> OnValueChanged { get; }
        
        public bool InitialValue { get; }
        
        public ToggleViewContext(bool initialValue, Action<bool> onValueChanged)
        {
            InitialValue = initialValue;
            OnValueChanged = onValueChanged;
        }
    }
}