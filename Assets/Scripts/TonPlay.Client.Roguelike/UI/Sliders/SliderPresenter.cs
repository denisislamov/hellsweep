using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Sliders.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Sliders
{
    public class SliderPresenter : Presenter<ISliderView, ISliderContext>, ISliderPresenter
    {
        private IDisposable _subscription;
        
        public SliderPresenter(ISliderView view, ISliderContext context) : base(view, context)
        {
            InitView();
            AddViewValueSubscription();
        }
        
        private void InitView()
        {
            View.SetSliderValue(Context.InitialValue);
        }

        public override void Dispose()
        {
            _subscription?.Dispose();
            base.Dispose();
        }

        private void AddViewValueSubscription()
        {
            _subscription = View.OnValueChanged.Subscribe(value => Context.OnValueChanged?.Invoke(value));
        }
        
        internal class Factory : PlaceholderFactory<ISliderView, ISliderContext, SliderPresenter>
        {
        }
    }
}