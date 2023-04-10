using System;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Sliders.Interfaces
{
    public interface ISliderContext : IScreenContext
    {
        float InitialValue { get; }
        
        Action<float> OnValueChanged { get; }
    }
}