using TonPlay.Client.Roguelike.UI.Samples.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Samples
{
    [CreateAssetMenu(fileName = nameof(SimplePopupScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(SimplePopupScreenInstaller))]
    public class SimplePopupScreenInstaller : ScreenInstaller
    {
        public override void InstallBindings()
        {
            var subContainer = Container.CreateSubContainer();

            Bind(subContainer);
            
            Container.Bind<IScreenFactory<ISimplePopupScreenContext, SimplePopupScreen>>()
                .FromSubContainerResolve()
                .ByInstance(subContainer)
                .AsCached();
        }

        private void Bind(DiContainer subContainer)
        {
            subContainer.Bind<IScreenFactory<ISimplePopupScreenContext, SimplePopupScreen>>()
                .To<SimplePopupScreen.Factory>()
                .AsCached()
                .WithArguments(ScreenPrefab);
            
            subContainer.BindFactory<ISimplePopupView, ISimplePopupScreenContext, SimplePopupPresenter, SimplePopupPresenter.Factory>();
        }
    }
}