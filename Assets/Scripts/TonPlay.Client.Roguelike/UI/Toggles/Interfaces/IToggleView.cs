using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Toggles.Interfaces
{
    public interface IToggleView : IView
    {
        void SetToggleValue(bool value);
    }
}