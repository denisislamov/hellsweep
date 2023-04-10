using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Toggles
{
    public class TogglePresenter : Presenter<IToggleView, IToggleContext>, ITogglePresenter
    {
        public TogglePresenter(IToggleView view, IToggleContext context) : base(view, context)
        {
        }
        
        internal class Factory : PlaceholderFactory<IToggleView, IToggleContext, TogglePresenter>
        {
        }
    }
}