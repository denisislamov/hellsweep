using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    [CreateAssetMenu(fileName = nameof(MergeScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(MergeScreenInstaller))]
    public class MergeScreenInstaller : ScreenInstaller
    {
        public override void InstallBindings()
        {
            var subContainer = Container.CreateSubContainer();

            Bind(subContainer);
            
            Container.Bind<IScreenFactory<IMergeScreenContext, MergeScreen>>()
                .FromSubContainerResolve()
                .ByInstance(subContainer)
                .AsCached();
        }
        
        private void Bind(DiContainer subContainer)
        {
            subContainer.Bind<IScreenFactory<IMergeScreenContext, MergeScreen>>()
                .To<MergeScreen.Factory>()
                .AsCached()
                .WithArguments(ScreenPrefab);
            
            subContainer.BindFactory<IMergeView, IMergeScreenContext ,MergePresenter, MergePresenter.Factory>();
        }
    }
}