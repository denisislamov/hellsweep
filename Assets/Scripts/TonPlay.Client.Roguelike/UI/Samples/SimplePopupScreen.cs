using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Samples.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Samples
{
    public class SimplePopupScreen : Screen<SimplePopupScreenContext>
    {
        [SerializeField] private SimplePopupView _view;
        
        [Inject]
        private void Construct(SimplePopupPresenter.Factory factory)
        {
            var presenter = factory.Create(_view, Context);
            Presenters.Add(presenter);
        }
        
        public class Factory : ScreenFactory<ISimplePopupScreenContext, SimplePopupScreen>
        {
            public Factory(DiContainer container, Screen screenPrefab)
                : base(container, screenPrefab)
            {
            }
        }
    }
}