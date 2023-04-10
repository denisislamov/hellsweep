using System;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Toggles.Interfaces
{
    public interface IToggleContext : IScreenContext
    {
        Action<bool> OnValueChanged { get; }
    }
}