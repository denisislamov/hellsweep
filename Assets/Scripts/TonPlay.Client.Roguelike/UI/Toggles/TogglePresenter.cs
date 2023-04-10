using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Toggles
{
    public class TogglePresenter : Presenter<IToggleView, IToggleContext>, ITogglePresenter
    {
        private IDisposable _subscription;
        
        public TogglePresenter(IToggleView view, IToggleContext context) : base(view, context)
        {
            InitView();
            AddViewValueSubscription();
        }

        public override void Dispose()
        {
            _subscription?.Dispose();
            base.Dispose();
        }

        private void InitView()
        {
            View.SetToggleValue(Context.InitialValue);
        }

        private void AddViewValueSubscription()
        {
            _subscription = View.OnValueChanged.Subscribe(value => Context.OnValueChanged?.Invoke(value));
        }

        internal class Factory : PlaceholderFactory<IToggleView, IToggleContext, TogglePresenter>
        {
        }
    }
}