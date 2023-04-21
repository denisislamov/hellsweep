using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    public class MergeScreen : Screen<MergeScreenContext>
    {
        [SerializeField] private MergeView _view;
        
        [Inject]
        private void Construct(MergePresenter.Factory factory)
        {
            var presenter = factory.Create(_view, Context);
            Presenters.Add(presenter);
        }
        
        public class Factory : ScreenFactory<IMergeScreenContext, MergeScreen>
        {
            public Factory(DiContainer container, Screen screenPrefab)
                : base(container, screenPrefab)
            {
            }
        }
    }
}