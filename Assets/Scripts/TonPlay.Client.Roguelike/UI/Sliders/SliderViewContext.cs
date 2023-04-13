using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Sliders
{
    public class SliderViewContext : ScreenContext, ISliderContext
    {
        public float InitialValue { get; }
        public Action<float> OnValueChanged { get; }
        
        public SliderViewContext(float initialValue, Action<float> onValueChanged)
        {
            InitialValue = initialValue;
            OnValueChanged = onValueChanged;
        }
    }
}