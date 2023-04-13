using System;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Sliders.Interfaces
{
    public interface ISliderView : IView
    {
        IObservable<float> OnValueChanged { get; }

        void SetSliderValue(float value);
    }
}