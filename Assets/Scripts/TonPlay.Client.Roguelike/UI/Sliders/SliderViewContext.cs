using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Sliders
{
    public class SliderViewContext : ScreenContext, ISliderContext
    {
        public Action<float> OnValueChanged { get; set; }
    }
}