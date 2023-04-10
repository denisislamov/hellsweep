using System;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Toggles.Interfaces
{
    public interface IToggleView : IView
    {
        IObservable<bool> OnValueChanged { get; }

        void SetToggleValue(bool value);
    }
}