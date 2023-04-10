using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.GameSettings
{
    [CreateAssetMenu(fileName = nameof(GameSettingsScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(GameSettingsScreenInstaller))]
    public class GameSettingsScreenInstaller : ScreenInstaller
    {
        public override void InstallBindings()
        {
            var subContainer = Container.CreateSubContainer();

            Bind(subContainer);
            
            Container.Bind<IScreenFactory<IGameSettingsScreenContext, GameSettingsScreen>>()
                     .FromSubContainerResolve()
                     .ByInstance(subContainer)
                     .AsCached();
        }

        private void Bind(DiContainer subContainer)
        {
            subContainer.Bind<IScreenFactory<IGameSettingsScreenContext, GameSettingsScreen>>()
                .To<GameSettingsScreen.Factory>()
                .AsCached()
                .WithArguments(ScreenPrefab);
            
            subContainer.BindFactory<IGameSettingsView, IGameSettingsScreenContext,GameSettingsPresenter, GameSettingsPresenter.Factory>();
        }
    }
}