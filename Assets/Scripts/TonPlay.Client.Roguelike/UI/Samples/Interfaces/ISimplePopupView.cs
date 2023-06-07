using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Samples.Interfaces
{
    public interface ISimplePopupView : IView
    {
        IButtonView CloseButton { get; }
    }
}