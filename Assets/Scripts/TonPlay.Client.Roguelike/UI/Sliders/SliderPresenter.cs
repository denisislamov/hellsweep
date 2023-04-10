using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Sliders
{
    public class SliderPresenter : Presenter<ISliderView, ISliderContext>, ISliderPresenter
    {
        public SliderPresenter(ISliderView view, ISliderContext context) : base(view, context)
        {
        }
        
        internal class Factory : PlaceholderFactory<ISliderView, ISliderContext, SliderPresenter>
        {
        }
    }
}