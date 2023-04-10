using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Sliders.Interfaces
{
    public interface ISliderView : IView
    {
        void SetSliderValue(float value);
    }
}